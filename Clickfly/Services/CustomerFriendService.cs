using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;

namespace clickfly.Services
{
    public class CustomerFriendService : ICustomerFriendService
    {
        private readonly ICustomerFriendRepository _customerFriendRepository;

        public CustomerFriendService(ICustomerFriendRepository customerFriendRepository)
        {
            _customerFriendRepository = customerFriendRepository;
        }

        public async Task Delete(string id)
        {
            await _customerFriendRepository.Delete(id);
            return;
        }

        public async Task<CustomerFriend> GetById(string id)
        {
            CustomerFriend customerFriend = await _customerFriendRepository.GetById(id);
            return customerFriend;
        }

        public async Task<PaginationResult<CustomerFriend>> Pagination(PaginationFilter filter)
        {
            PaginationResult<CustomerFriend> paginationResult = await _customerFriendRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<CustomerFriend> Save(CustomerFriend customerFriend)
        {
            bool update = customerFriend.id != "";

            if(update)
            {
                customerFriend = await _customerFriendRepository.Update(customerFriend);
            }
            else
            {
                customerFriend = await _customerFriendRepository.Create(customerFriend);
            }

            return customerFriend;
        }
    }
}
