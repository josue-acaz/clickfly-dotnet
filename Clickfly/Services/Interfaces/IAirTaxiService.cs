using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IAirTaxiService
    {
        Task<AirTaxi> Save(AirTaxi airTaxi);
        Task<AirTaxi> GetById(string id);
        Task<AirTaxi> GetByAccessToken(string token);
        Task<PaginationResult<AirTaxi>> Pagination(PaginationFilter filter);
    }
}