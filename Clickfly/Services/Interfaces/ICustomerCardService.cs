using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using PagarmeCoreApi.Standard.Models;

namespace clickfly.Services
{
    public interface ICustomerCardService
    {
        Task<CustomerCard> Save(CustomerCard customerCard);
        Task<PaginationResult<CustomerCard>> Pagination(PaginationFilter filter);
        Task<CustomerCard> GetById(string id);
        Task Delete(string id);
    }
}
