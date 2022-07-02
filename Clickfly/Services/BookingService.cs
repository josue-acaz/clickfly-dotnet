using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Exceptions;
using clickfly.Helpers;
using PagarmeCoreApi.Standard.Models;
using PagarmeCoreApi.Standard.Exceptions;
using PagarmeCoreApi.Standard.Controllers;
using System.Collections.Generic;
using System.Linq;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class BookingService : BaseService, IBookingService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightSegmentRepository _flightSegmentRepository;
        private readonly ICustomerCardRepository _customerCardRepository;
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly IBookingPaymentRepository _bookingPaymentRepository;
        private readonly ICustomerFriendRepository _customerFriendRepository;
        private readonly IBookingStatusRepository _bookingStatusRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IOrdersController _ordersController;
        private readonly ICustomersController _customersController;

        public BookingService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository, 
            IPermissionRepository permissionRepository, 
            INotificator notificator, 
            IInformer informer, 
            IUtils utils,
            ICustomerRepository customerRepository,
            IBookingRepository bookingRepository,
            IFlightSegmentRepository flightSegmentRepository,
            ICustomerCardRepository customerCardRepository,
            ICustomerAddressRepository customerAddressRepository,
            IBookingPaymentRepository bookingPaymentRepository,
            ICustomerFriendRepository customerFriendRepository,
            IBookingStatusRepository bookingStatusRepository,
            IPassengerRepository passengerRepository,
            ITicketRepository ticketRepository,
            IOrdersController ordersController,
            ICustomersController customersController
        ) : base(
            appSettings, 
            systemLogRepository, 
            permissionRepository, 
            notificator, 
            informer, 
            utils
        )
        {
            _customerRepository = customerRepository;
            _bookingRepository = bookingRepository;
            _flightSegmentRepository = flightSegmentRepository;
            _customerCardRepository = customerCardRepository;
            _customerAddressRepository = customerAddressRepository;
            _bookingPaymentRepository = bookingPaymentRepository;
            _customerFriendRepository = customerFriendRepository;
            _bookingStatusRepository = bookingStatusRepository;
            _passengerRepository = passengerRepository;
            _ticketRepository = ticketRepository;
            _ordersController = ordersController;
            _customersController = customersController;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<Booking>> Pagination(PaginationFilter filter)
        {
            string customer_id = _informer.GetValue(UserIdTypes.CustomerId);
            filter.customer_id = customer_id;

            PaginationResult<Booking> paginationResult = await _bookingRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<Booking> Save(Booking booking)
        {
            string bookingId = Guid.NewGuid().ToString();
            string bookingPaymentId = Guid.NewGuid().ToString();
            string customerId = _informer.GetValue(UserIdTypes.CustomerId);

            // Adicionar id da reserva, cliente etc
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("customer_id", customerId);
            metadata.Add("booking_id", bookingId);

            Customer customer = await _customerRepository.GetById(customerId);
            FlightSegment flightSegment = await _flightSegmentRepository.GetById(booking.flight_segment_id);
            
            // Verificar assentos disponíveis
            int quantity = booking.selected_passengers.Length + (booking.customer_is_passenger ? 1 : 0);
            if(quantity > flightSegment.available_seats)
            {
                Notify("Quantidade disponível de assentos excedida. Por favor, verifique os assentos disponíveis e tente novamente.");
                return null;
            }

            CreateOrderRequest orderRequest = new CreateOrderRequest();

            // Nó payment
            List<CreatePaymentRequest> paymentRequests = new List<CreatePaymentRequest>();
            CreatePaymentRequest paymentRequest = new CreatePaymentRequest();
            GetCustomerResponse customerResponse = await _customersController.GetCustomerAsync(customer.customer_id);

            if(booking.payment_method == PaymentMethods.CreditCard)
            {
                int installments = booking.installments;
                if(installments < 0)
                {
                    Notify("Número mínimo permitido para parcelamento: 1");
                    return null;
                }

                if(installments > 12)
                {
                    Notify("Número máximo permitido para parcelamento: 12");
                    return null;
                }

                CustomerCard customerCard = await _customerCardRepository.GetById(booking.customer_card_id);
                GetCardResponse cardResponse = await _customersController.GetCardAsync(customer.customer_id, customerCard.card_id);
                
                if(customerCard == null || cardResponse == null)
                {
                    Notify("Cartão é obrigatório.");
                }

                CustomerAddress customerAddress = await _customerAddressRepository.GetById(booking.customer_address_id);
                GetAddressResponse addressResponse = await _customersController.GetAddressAsync(customer.customer_id, customerAddress.address_id);
                
                if(customerAddress == null || addressResponse == null)
                {
                    Notify("Endereço de cobrança é obrigatório.");
                }

                // Atualizar o endereço de cobrança
                UpdateCardRequest updateCardRequest = new UpdateCardRequest();
                updateCardRequest.ExpMonth = cardResponse.ExpMonth;
                updateCardRequest.ExpYear = cardResponse.ExpYear;
                updateCardRequest.HolderName = cardResponse.HolderName;
                updateCardRequest.BillingAddressId = customerAddress.address_id;
                
                try
                {
                    cardResponse = await _customersController.UpdateCardAsync(customer.customer_id, customerCard.card_id, updateCardRequest);
                }
                catch(ErrorException ex)
                {
                    Console.WriteLine(ex.Errors);
                }

                CreateCreditCardPaymentRequest createCreditCardPayment = new CreateCreditCardPaymentRequest();
                createCreditCardPayment.Installments = installments;
                createCreditCardPayment.StatementDescriptor = "ClickFly Pass"; // Nome que aparece na fatura do cartão
                createCreditCardPayment.OperationType = "auth_and_capture";
                createCreditCardPayment.MerchantCategoryCode = 4722;
                createCreditCardPayment.CardId = customerCard.card_id;
                createCreditCardPayment.Card = new CreateCardRequest{ Cvv = booking.card_cvv };
            
                paymentRequest.PaymentMethod = PaymentMethods.CreditCard;
                paymentRequest.CreditCard = createCreditCardPayment;
                // Adicionar IP, Lat e LNG aos dados da compra
                // Verifcar anti-fraude
                // ID da sessão do usuário
                // Dispositivo
            }
            else if(booking.payment_method == PaymentMethods.Pix)
            {
                CreatePixPaymentRequest pixPaymentRequest = new CreatePixPaymentRequest();
                pixPaymentRequest.ExpiresIn = 52134613;

                paymentRequest.PaymentMethod = PaymentMethods.Pix;
                paymentRequest.Pix = pixPaymentRequest;
            }
            else
            {
                Notify("Meio de pagamento não disponível.");
                return null;
            }

            paymentRequests.Add(paymentRequest);

            // Item do pedido
            List<CreateOrderItemRequest> orderItemRequests = new List<CreateOrderItemRequest>();
            CreateOrderItemRequest orderItemRequest = new CreateOrderItemRequest();

            orderItemRequest.Code = flightSegment.id;
            orderItemRequest.Amount = flightSegment.price_per_seat;
            orderItemRequest.Description = "Pass ClickFly";
            orderItemRequest.Quantity = quantity;
            orderItemRequests.Add(orderItemRequest);

            // Agrupar dados
            orderRequest.Code = bookingPaymentId;
            orderRequest.Items = orderItemRequests;
            orderRequest.CustomerId = customer.customer_id;
            orderRequest.Payments = paymentRequests;

            // Solicitar compra
            try
            {
                GetOrderResponse orderResponse = await _ordersController.CreateOrderAsync(orderRequest);
                GetChargeResponse chargeResponse = orderResponse.Charges[orderResponse.Charges.Count - 1];
                /*
                if(booking.payment_method == PaymentMethods.Pix)
                {
                    booking.pix_transaction_response = chargeResponse.LastTransaction as GetPixTransactionResponse;
                }
                */
                if(orderResponse.Status == OrderStatus.Failed)
                {
                    Notify("Ocorreu um erro ao processar seu pagamento. Por favor, verifique os dados de pagamento e tente novamente!");
                    return null;
                }

                // Reserva
                booking.id = bookingId;
                booking.customer_id = customer.id;
                booking = await _bookingRepository.Create(booking);

                // Associar pagamento à reserva
                BookingPayment bookingPayment = new BookingPayment();
                bookingPayment.id = bookingPaymentId;
                bookingPayment.order_id = orderResponse.Id;
                bookingPayment.booking_id = bookingId;
                bookingPayment.payment_method = booking.payment_method;
                bookingPayment = await _bookingPaymentRepository.Create(bookingPayment);

                // Criar passageiros (CUSTOMER)
                if(booking.customer_is_passenger)
                {
                    Passenger passenger = new Passenger();
                    passenger.name = customer.name;
                    passenger.email = customer.email;
                    passenger.birthdate = customer.birthdate;
                    passenger.document = customer.document;
                    passenger.document_type = customer.document_type;
                    passenger.booking_id = bookingId;
                    passenger.flight_segment_id = flightSegment.id;
                    passenger = await _passengerRepository.Create(passenger);

                    // Criar bilhete (CUSTOMER)
                    if(orderResponse.Status == OrderStatus.Paid)
                    {
                        Ticket ticket = new Ticket();
                        ticket.qr_code = "NULL";
                        ticket.passenger_id = passenger.id;
                        ticket.flight_segment_id = flightSegment.id;
                        ticket = await _ticketRepository.Create(ticket);
                    }
                }

                // Criar passageiros (CUSTOMER_FRIENDS)
                string[] passengers = new string[booking.selected_passengers.Length];
                for (int i = 0; i < booking.selected_passengers.Length; i++)
                {
                    string customerFriendId = booking.selected_passengers[i];
                    CustomerFriend customerFriend = await _customerFriendRepository.GetById(customerFriendId);

                    Passenger passenger = new Passenger();
                    passenger.name = customerFriend.name;
                    passenger.email = customerFriend.email;
                    passenger.birthdate = customerFriend.birthdate;
                    passenger.document = customerFriend.document;
                    passenger.document_type = customerFriend.document_type;
                    passenger.booking_id = bookingId;
                    passenger.flight_segment_id = flightSegment.id;
                    
                    passenger = await _passengerRepository.Create(passenger);
                    passengers[i] = passenger.id;
                }

                // Criar bilhetes (CUSTOMER_FRIENDS)
                if(orderResponse.Status == OrderStatus.Paid)
                {
                    for (int i = 0; i < passengers.Length; i++)
                    {
                        string passengerId = passengers[i];

                        Ticket ticket = new Ticket();
                        ticket.qr_code = "NULL";
                        ticket.passenger_id = passengerId;
                        ticket.flight_segment_id = flightSegment.id;
                        ticket = await _ticketRepository.Create(ticket);
                    }
                }

                // Criar status da reserva
                BookingStatus bookingStatus = new BookingStatus();
                bookingStatus.booking_id = bookingId;
                bookingStatus.type = _utils.GetBookingStatus(orderResponse.Status);
                bookingStatus = await _bookingStatusRepository.Create(bookingStatus);

                booking.status.Add(bookingStatus);
                booking.payments.Add(bookingPayment);
            }
            catch(ErrorException ex)
            {
                Console.WriteLine(ex.Errors);
            }

            return booking;
        }
    }
}
