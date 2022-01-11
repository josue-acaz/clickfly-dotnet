using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IAirTaxiBaseService
    {
        Task<AirTaxiBase> Save(AirTaxiBase airTaxiBase);
        Task<PaginationResult<AirTaxiBase>> Pagination(PaginationFilter filter);
        Task<AirTaxiBase> GetById(string id);
        Task Delete(string id);
    }
}
