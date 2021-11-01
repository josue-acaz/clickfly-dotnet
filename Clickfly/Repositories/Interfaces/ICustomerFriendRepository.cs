using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface ICustomerFriendRepository
    {
        Task<CustomerFriend> Create(CustomerFriend customerFriend);
        Task<CustomerFriend> GetById(string id);
        Task<IEnumerable<CustomerFriend>> BulkGetById(string[] ids);
        Task<CustomerFriend> Update(CustomerFriend customerFriend, string[] fields = null);
        Task Delete(string id);
        Task<PaginationResult<CustomerFriend>> Pagination(PaginationFilter filter);
    }
}
