using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface ICustomerCardRepository
    {
        Task<CustomerCard> Create(CustomerCard customerCard);
        Task<CustomerCard> GetById(string id);
        Task<CustomerCard> Update(CustomerCard customerCard);
        Task Delete(string id);
        Task<PaginationResult<CustomerCard>> Pagination(PaginationFilter filter);
    }
}
