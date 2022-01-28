using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using PagarmeCoreApi.Standard.Controllers;
using PagarmeCoreApi.Standard.Models;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using clickfly.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace clickfly.Services
{
    public class CustomerCardService : BaseService, ICustomerCardService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerCardRepository _customerCardRepository;
        private readonly ICustomersController _customersController;

        public CustomerCardService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator,
            IInformer informer,
            IUtils utils, 
            ICustomerRepository customerRepository, 
            ICustomerCardRepository customerCardRepository, 
            ICustomersController customersController
        ) : base(appSettings, systemLogRepository, permissionRepository, notificator, informer, utils)
        {
            _customerRepository = customerRepository;
            _customerCardRepository = customerCardRepository;
            _customersController = customersController;
        }

        public async Task Delete(string id)
        {
            await _customerCardRepository.Delete(id);
            return;
        }

        public async Task<CustomerCard> GetById(string id)
        {
            CustomerCard customerCard = await _customerCardRepository.GetById(id);
            
            string customerId = customerCard.customer_id;
            Customer customer = await _customerRepository.GetById(customerId);

            string customer_id = customer.customer_id;
            string card_id = customerCard.card_id;
            GetCardResponse cardResponse = await _customersController.GetCardAsync(customer_id, card_id);
            
            customerCard.brand = cardResponse.Brand;
            customerCard.holder_name = cardResponse.HolderName;
            customerCard.number = $"•••• {cardResponse.LastFourDigits}";
            customerCard.exp_date = _utils.GetExpCard(cardResponse.ExpMonth, cardResponse.ExpYear);

            return customerCard;
        }

        public async Task<PaginationResult<CustomerCard>> Pagination(PaginationFilter filter)
        {
            Customer authCustomer = _informer.GetValue<Customer>(UserTypes.Customer);
            filter.customer_id = authCustomer.id;

            Console.WriteLine($"SYSTEM CUSTOMER ID: {authCustomer.id}");
            Console.WriteLine($"ID X: {authCustomer.customer_id}");

            PaginationResult<CustomerCard> paginationResult = await _customerCardRepository.Pagination(filter);
            CustomerCard[] customerCards = paginationResult.data.ToArray();

            Customer customer = await _customerRepository.GetById(authCustomer.id);
            string customer_id = customer.customer_id;

            Console.WriteLine($"PAGAR.ME CUSTOMER ID: {customer_id}");
            
            for (int index = 0; index < customerCards.Length; index++)
            {
                CustomerCard customerCard = customerCards[index];
                Console.WriteLine($"CARD ID: {customerCard.card_id}");
                
                try
                {
                    GetCardResponse cardResponse = await _customersController.GetCardAsync("cus_8qdBkx2T2UdpnmZN", customerCard.card_id);
            
                    customerCard.brand = cardResponse.Brand;
                    customerCard.holder_name = cardResponse.HolderName;
                    customerCard.number = $"•••• {cardResponse.LastFourDigits}";
                    customerCard.exp_date = _utils.GetExpCard(cardResponse.ExpMonth, cardResponse.ExpYear);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return paginationResult;
        }

        public async Task<CustomerCard> Save(CustomerCard customerCard)
        {
            Customer customer = _informer.GetValue<Customer>(UserTypes.Customer);
            bool update = customerCard.id != "";

            if(update)
            {
                customerCard = await _customerCardRepository.Update(customerCard);
            }
            else
            {
                customerCard = await _customerCardRepository.Create(customerCard);

                var metadata = new Dictionary<string, string>();
                metadata.Add("customer_card_id", customerCard.id);

                //string[] exp_date = customerCard.exp_date.Split('/');
                //int exp_month = Int32.Parse(exp_date[0]);
                //int exp_year = Int32.Parse(exp_date[1]);

                CreateCardRequest createCardRequest = new CreateCardRequest();
                createCardRequest.Number = String.Join("", customerCard.number.Split(" "));
                createCardRequest.HolderName = customerCard.holder_name;
                createCardRequest.ExpMonth = 8;
                createCardRequest.ExpYear = 28;
                createCardRequest.Cvv = customerCard.cvv;
                createCardRequest.Metadata = metadata;

                GetCardResponse getCardResponse = await _customersController.CreateCardAsync(customer.customer_id, createCardRequest);
                customerCard.card_id = getCardResponse.Id;

                Console.WriteLine($"PAGAR.ME CARD ID: {customerCard.card_id}");

                await _customerCardRepository.Update(customerCard);
            }

            return customerCard;
        }
    }
}
