using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IDoubleCheckRepository
    {
        Task<DoubleCheck> Create(DoubleCheck doubleCheck);
        Task<DoubleCheck> GetById(string id);
        Task<DoubleCheck> GetByName(string name, string statePrefix);
        Task<DoubleCheck> Update(DoubleCheck doubleCheck);
        Task Delete(string id);
        Task<PaginationResult<DoubleCheck>> Pagination(PaginationFilter filter);
        Task Approve(DoubleCheck doubleCheck);
    }
}
