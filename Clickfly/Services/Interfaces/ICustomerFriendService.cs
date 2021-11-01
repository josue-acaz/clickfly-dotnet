using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

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
