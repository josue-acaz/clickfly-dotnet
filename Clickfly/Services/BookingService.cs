using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using clickfly.Exceptions;
using clickfly.Helpers;
using PagarmeCoreApi.Standard.Models;
using PagarmeCoreApi.Standard.Controllers;
using System.Collections.Generic;
using System.Linq;

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

        public BookingService(
            IOptions<AppSettings> appSettings, 
            INotificator notificator,
            IInformer informer,
            IUtils utils, 
            IBookingRepository bookingRepository, 
            ICustomerRepository customerRepository, 
            IFlightSegmentRepository flightSegmentRepository, 
            ICustomerCardRepository customerCardRepository, 
            ICustomerAddressRepository customerAddressRepository,
            IBookingStatusRepository bookingStatusRepository,
            IBookingPaymentRepository bookingPaymentRepository,
            ICustomerFriendRepository customerFriendRepository,
            IPassengerRepository passengerRepository,
            ITicketRepository ticketRepository,
            IOrdersController ordersController
        ) : base(appSettings, notificator, informer, utils)
        {
            _customerRepository = customerRepository;
            _flightSegmentRepository = flightSegmentRepository;
            _customerCardRepository = customerCardRepository;
            _customerAddressRepository = customerAddressRepository;
            _bookingRepository = bookingRepository;
            _bookingPaymentRepository = bookingPaymentRepository;
            _customerFriendRepository = customerFriendRepository;
            _passengerRepository = passengerRepository;
            _bookingStatusRepository = bookingStatusRepository;
            _ticketRepository = ticketRepository;

            _ordersController = ordersController;
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
            Customer customer = _informer.GetValue<Customer>(UserTypes.Customer);

            filter.customer_id = customer.id;
            PaginationResult<Booking> paginationResult = await _bookingRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<CreateBookingResponse> Save(Booking booking)
        {
            CreateBookingResponse createBookingResponse = new CreateBookingResponse();

            string customer_id = booking.customer_id;
            string customerCardId = booking.customer_card_id;
            string flightSegmentId = booking.flight_segment_id;
            string customerAddressId = booking.customer_address_id;

            string[] selected_passengers = booking.selected_passengers;
            string paymentMethod = booking.payment_method;
            int installments = booking.installments;
            bool customerIsPassenger = booking.customer_is_passenger;

            Customer customer = await _customerRepository.GetById(customer_id);
            FlightSegment flightSegment = await _flightSegmentRepository.GetById(flightSegmentId);
            CustomerAddress customerAddress = await _customerAddressRepository.GetById(customerAddressId);

            if(
                (customerIsPassenger && customer.type != CustomerTypes.Individual) ||
                (flightSegment == null || customerAddress == null) ||
                (customer == null || !customer.verified)
            )
            {
                throw new BadRequestException("Requisição inválida.");
            }

            int availableSeats = flightSegment.available_seats;
            int selectedSeats = selected_passengers.Length + (customerIsPassenger ? 1 : 0);
            float pricePerSeat = flightSegment.price_per_seat;
            string addressId = customerAddress.address_id;
            string customerId = customer.customer_id;
            
            if(selectedSeats > availableSeats)
            {
                throw new BadRequestException("Requisição inválida.");
            }

            float amount = selectedSeats * pricePerSeat;
            CreateOrderRequest orderRequest = new CreateOrderRequest();

            List<CreateOrderItemRequest> createOrderItems = new List<CreateOrderItemRequest>();
            CreateOrderItemRequest createOrderItemRequest = new CreateOrderItemRequest();
            createOrderItemRequest.Description = "Passagem ClickFly";
            createOrderItemRequest.Quantity = selectedSeats;
            createOrderItemRequest.Amount = (int)pricePerSeat;
            createOrderItems.Add(createOrderItemRequest);
            orderRequest.Items = createOrderItems;
            
            List<CreatePaymentRequest> createPayments = new List<CreatePaymentRequest>();
            CreatePaymentRequest paymentRequest = new CreatePaymentRequest();

            if(paymentMethod == PaymentMethods.CreditCard)
            {
                CustomerCard customerCard = await _customerCardRepository.GetById(customerCardId);
                if(
                    customerCard == null || 
                    (installments < 1 || installments > 12)
                )
                {
                    throw new BadRequestException("Requisição inválida.");
                }

                string cardId = customerCard.card_id;
                CreateCreditCardPaymentRequest creditCardPaymentRequest = new CreateCreditCardPaymentRequest();
                creditCardPaymentRequest.Capture = true;
                creditCardPaymentRequest.Installments = installments;
                creditCardPaymentRequest.StatementDescriptor = "ClickFly Pass";
                creditCardPaymentRequest.Recurrence = false;
                creditCardPaymentRequest.CardId = cardId;

                paymentRequest.CreditCard = creditCardPaymentRequest;
            }
            else if(paymentMethod == PaymentMethods.Pix)
            {
                CreatePixPaymentRequest pixPaymentRequest = new CreatePixPaymentRequest();
                pixPaymentRequest.ExpiresIn = 52134613;

                paymentRequest.Pix = pixPaymentRequest;
            }
            else
            {
                throw new BadRequestException("Requisição inválida.");
            }

            paymentRequest.PaymentMethod = paymentMethod;
            createPayments.Add(paymentRequest);
            orderRequest.Payments = createPayments;
            orderRequest.CustomerId = customerId;

            GetOrderResponse orderResponse = await _ordersController.CreateOrderAsync(orderRequest);
            GetChargeResponse chargeResponse = orderResponse.Charges[0];
            GetTransactionResponse transactionResponse = chargeResponse.LastTransaction;
            
            string orderId = orderResponse.Id;
            string bookingStatus = _utils.GetBookingStatus(chargeResponse.Status);

            // CASO PAGAMENTO NÃO SEJA APROVADO, NÃO CRIAR RESERVA
            if(bookingStatus == BookingStatusTypes.NotApproved)
            {
                return createBookingResponse;
            }

            Booking createBooking = new Booking();
            createBooking.customer_id = customer_id;
            createBooking.flight_segment_id = flightSegmentId;
            createBooking = await _bookingRepository.Create(createBooking);

            string bookingId = createBooking.id;
            BookingPayment bookingPayment = new BookingPayment();
            bookingPayment.order_id = orderId;
            bookingPayment.booking_id = bookingId;
            bookingPayment.payment_method = paymentMethod;
            bookingPayment = await _bookingPaymentRepository.Create(bookingPayment);

            IEnumerable<CustomerFriend> customerFriends = await _customerFriendRepository.BulkGetById(selected_passengers);
            CustomerFriend[] customerFriendPassengers = customerFriends.ToArray<CustomerFriend>();

            if(customerIsPassenger)
            {
                Passenger passenger = new Passenger();
                passenger.name = customer.name;
                passenger.email = customer.email;
                passenger.birthdate = customer.birthdate;
                passenger.document = customer.document;
                passenger.document_type = customer.document_type;
                passenger.booking_id = bookingId;
                passenger.flight_segment_id = flightSegmentId;

                string boardingUrl = $"https://www.dashboard.clickfly.app/boardings/{flightSegmentId}/passengers/{passenger.id}";

                // Adicionar os bilhetes de passagem se o status de reserva for "APPROVED"
                if(bookingStatus == BookingStatusTypes.Approved)
                {
                    Ticket ticket = new Ticket();
                    ticket.id = Guid.NewGuid().ToString();
                    ticket.qr_code = boardingUrl/*_utils.GenerateQRCode(boardingUrl)*/;
                    ticket.passenger_id = passenger.id;
                    ticket.flight_segment_id = flightSegmentId;
                    passenger.ticket = ticket;
                }

                await _passengerRepository.Create(passenger);
            }
            
            List<Passenger> Passengers = new List<Passenger>();
            for (int index = 0; index < customerFriendPassengers.Length; index++)
            {
                CustomerFriend customerFriendPassenger = customerFriendPassengers[index];

                Passenger passenger = new Passenger();
                passenger.id = Guid.NewGuid().ToString();
                passenger.name = customerFriendPassenger.name;
                passenger.email = customerFriendPassenger.email;
                passenger.birthdate = customerFriendPassenger.birthdate;
                passenger.document = customerFriendPassenger.document;
                passenger.document_type = customerFriendPassenger.document_type;
                passenger.booking_id = bookingId;
                passenger.flight_segment_id = flightSegmentId;

                string boardingUrl = $"https://www.dashboard.clickfly.app/boardings/{flightSegmentId}/passengers/{passenger.id}";

                // Adicionar os bilhetes de passagem se o status de reserva for "APPROVED"
                if(bookingStatus == BookingStatusTypes.Approved)
                {
                    Ticket ticket = new Ticket();
                    ticket.id = Guid.NewGuid().ToString();
                    ticket.qr_code = boardingUrl/*_utils.GenerateQRCode(boardingUrl)*/;
                    ticket.passenger_id = passenger.id;
                    ticket.flight_segment_id = flightSegmentId;
                    passenger.ticket = ticket;
                }

                Passengers.Add(passenger);
            }

            Passenger[] _passengers = Passengers.ToArray<Passenger>();
            if(_passengers.Length > 0)
            {
                await _passengerRepository.RangeCreate(_passengers);
            }

            BookingStatus createBookingStatus = new BookingStatus();
            createBookingStatus.type = bookingStatus;
            createBookingStatus.booking_id = bookingId;

            createBookingStatus = await _bookingStatusRepository.Create(createBookingStatus);
            createBookingResponse.booking_status = createBookingStatus;
            createBookingResponse.payment_transaction = transactionResponse;

            return createBookingResponse;
        }
    }
}
