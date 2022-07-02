using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using PagarmeCoreApi.Standard.Controllers;
using PagarmeCoreApi.Standard.Models;
using Microsoft.Extensions.Options;
using PagarmeCoreApi.Standard.Exceptions;
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

            try
            {
                GetCardResponse cardResponse = await _customersController.GetCardAsync(customer.customer_id, customerCard.card_id);
            
                customerCard.brand = cardResponse.Brand;
                customerCard.exp_year = cardResponse.ExpYear;
                customerCard.exp_month = cardResponse.ExpMonth;
                customerCard.holder_name = cardResponse.HolderName;
                customerCard.number = $"•••• {cardResponse.LastFourDigits}";
                customerCard.exp_date = _utils.GetExpCard(cardResponse.ExpMonth, cardResponse.ExpYear);
            }
            catch(ErrorException ex)
            {
                Console.WriteLine(ex.Errors);
            }

            return customerCard;
        }

        public async Task<PaginationResult<CustomerCard>> Pagination(PaginationFilter filter)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(filter));
            filter.customer_id = _informer.GetValue(UserIdTypes.CustomerId);
            Customer customer = await _customerRepository.GetById(filter.customer_id);
            ListCardsResponse cardsResponse = await _customersController.GetCardsAsync(customer.customer_id, filter.page_number, filter.page_size);
            List<CustomerCard> customerCards = new List<CustomerCard>();

            List<GetCardResponse> cards = cardsResponse.Data;
            foreach(GetCardResponse cardResponse in cards)
            {
                string customer_card_id = cardResponse.Metadata["customer_card_id"];
                CustomerCard customerCard = await _customerCardRepository.GetById(customer_card_id);
                customerCard.brand = cardResponse.Brand;
                customerCard.exp_year = cardResponse.ExpYear;
                customerCard.exp_month = cardResponse.ExpMonth;
                customerCard.holder_name = cardResponse.HolderName;
                customerCard.number = $"•••• {cardResponse.LastFourDigits}";
                customerCard.exp_date = _utils.GetExpCard(cardResponse.ExpMonth, cardResponse.ExpYear);

                customerCards.Add(customerCard);
            }

            PaginationResult<CustomerCard> paginationResult = _utils.CreatePaginationResult<CustomerCard>(customerCards, filter, customerCards.Count);
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

                string number = String.Join("", customerCard.number.Split(" "));
                string[] exp_date = customerCard.exp_date.Split('/');
                int exp_month = Int32.Parse(exp_date[0]);
                int exp_year = Int32.Parse(exp_date[1]);

                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("customer_card_id", customerCard.id);

                CreateCardRequest createCardRequest = new CreateCardRequest();
                createCardRequest.Number = number;
                createCardRequest.ExpYear = exp_year;
                createCardRequest.ExpMonth = exp_month;
                createCardRequest.Cvv = customerCard.cvv;
                createCardRequest.HolderName = customerCard.holder_name;
                createCardRequest.Metadata = metadata;

                GetCardResponse getCardResponse = await _customersController.CreateCardAsync(customer.customer_id, createCardRequest);
                customerCard.card_id = getCardResponse.Id;

                await _customerCardRepository.Update(customerCard);
            }

            return customerCard;
        }
    }
}
