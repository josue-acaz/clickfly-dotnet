using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class CustomerFriendService : BaseService, ICustomerFriendService
    {
        private readonly ICustomerFriendRepository _customerFriendRepository;

        public CustomerFriendService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            ICustomerFriendRepository customerFriendRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
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
