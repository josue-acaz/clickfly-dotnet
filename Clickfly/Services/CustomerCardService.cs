using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using PagarmeCoreApi.Standard.Controllers;
using PagarmeCoreApi.Standard.Models;
using Microsoft.Extensions.Options;

namespace clickfly.Services
{
    public class CustomerCardService : BaseService, ICustomerCardService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerCardRepository _customerCardRepository;
        private readonly ICustomersController _customersController;

        public CustomerCardService(
            IOptions<AppSettings> appSettings, 
            IUtils utils, 
            ICustomerRepository customerRepository, 
            ICustomerCardRepository customerCardRepository, 
            ICustomersController customersController
        ) : base(appSettings, utils)
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
            customerCard.card_number = $"•••• {cardResponse.LastFourDigits}";
            customerCard.exp_date = _utils.GetExpCard(cardResponse.ExpMonth, cardResponse.ExpYear);

            return customerCard;
        }

        public async Task<PaginationResult<CustomerCard>> Pagination(PaginationFilter filter)
        {
            string customerId = filter.customer_id;
            PaginationResult<CustomerCard> paginationResult = await _customerCardRepository.Pagination(filter);
            CustomerCard[] customerCards = paginationResult.data.ToArray();

            Customer customer = await _customerRepository.GetById(customerId);
            string customer_id = customer.customer_id;

            ListCardsResponse listCardsResponse = await _customersController.GetCardsAsync(customer_id, filter.page_number, filter.page_size);
            GetCardResponse[] cardsResponse = listCardsResponse.Data.ToArray();

            for (int index = 0; index < customerCards.Length; index++)
            {
                CustomerCard customerCard = customerCards[index];
                GetCardResponse cardResponse = Array.Find<GetCardResponse>(cardsResponse, cardsResponse => cardsResponse.Id == customerCard.card_id);
            
                customerCard.brand = cardResponse.Brand;
                customerCard.holder_name = cardResponse.HolderName;
                customerCard.card_number = $"•••• {cardResponse.LastFourDigits}";
                customerCard.exp_date = _utils.GetExpCard(cardResponse.ExpMonth, cardResponse.ExpYear);
            }

            return paginationResult;
        }

        public async Task<CustomerCard> Save(CustomerCard customerCard)
        {
            bool update = customerCard.id != "";

            if(update)
            {
                customerCard = await _customerCardRepository.Update(customerCard);
            }
            else
            {
                customerCard = await _customerCardRepository.Create(customerCard);
            }

            return customerCard;
        }
    }
}
