using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IAirTaxiBaseRepository
    {
        Task<AirTaxiBase> Create(AirTaxiBase airTaxiBase);
        Task<AirTaxiBase> GetById(string id);
        Task<AirTaxiBase> Update(AirTaxiBase airTaxiBase);
        Task Delete(string id);
        Task<PaginationResult<AirTaxiBase>> Pagination(PaginationFilter filter);
    }
}