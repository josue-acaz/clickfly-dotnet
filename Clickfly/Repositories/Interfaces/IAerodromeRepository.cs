using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IAerodromeRepository
    {
        Task<Aerodrome> Create(Aerodrome aerodrome);
        Task<Aerodrome> GetById(string id);
        Task<Aerodrome> Update(Aerodrome aerodrome);
        Task Delete(string id);
        Task<PaginationResult<Aerodrome>> Pagination(PaginationFilter filter);
    }
}
