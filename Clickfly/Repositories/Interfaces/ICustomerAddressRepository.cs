using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface ICustomerAddressRepository
    {
        Task<CustomerAddress> Create(CustomerAddress customerAddress);
        Task<CustomerAddress> GetById(string id);
        Task<CustomerAddress> Update(CustomerAddress customerAddress);
        Task Delete(string id);
        Task<PaginationResult<CustomerAddress>> Pagination(PaginationFilter filter);
    }
}
