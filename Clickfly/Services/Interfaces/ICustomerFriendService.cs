using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface ICustomerFriendService
    {
        Task<CustomerFriend> Save(CustomerFriend customerFriend);
        Task<PaginationResult<CustomerFriend>> Pagination(PaginationFilter filter);
        Task<CustomerFriend> GetById(string id);
        Task Delete(string id);
    }
}
