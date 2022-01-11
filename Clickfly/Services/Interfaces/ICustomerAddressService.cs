using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface ICustomerAddressService
    {
        Task<CustomerAddress> Save(CustomerAddress customerAddress);
        Task<PaginationResult<CustomerAddress>> Pagination(PaginationFilter filter);
        Task<CustomerAddress> GetById(string id);
        Task Delete(string id);
    }
}
