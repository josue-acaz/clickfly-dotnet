using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

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
