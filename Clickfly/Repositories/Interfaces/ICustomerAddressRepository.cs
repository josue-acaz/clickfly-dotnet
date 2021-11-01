using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface ICustomerAddressRepository
    {
        Task<CustomerAddress> Create(CustomerAddress customerAddress);
        Task<CustomerAddress> GetById(string id);
        Task<CustomerAddress> Update(CustomerAddress customerAddress, string[] fields = null);
        Task Delete(string id);
        Task<PaginationResult<CustomerAddress>> Pagination(PaginationFilter filter);
    }
}
