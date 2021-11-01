using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface ICustomerCardRepository
    {
        Task<CustomerCard> Create(CustomerCard customerCard);
        Task<CustomerCard> GetById(string id);
        Task<CustomerCard> Update(CustomerCard customerCard, string[] fields = null);
        Task Delete(string id);
        Task<PaginationResult<CustomerCard>> Pagination(PaginationFilter filter);
    }
}
