using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IAirTaxiRepository
    {
        Task<AirTaxi> Create(AirTaxi airTaxi);
        Task<AirTaxi> GetById(string id, string[] fields = null);
        Task Update(AirTaxi airTaxi);
        Task Delete(string id);
        Task<PaginationResult<AirTaxi>> Pagination(PaginationFilter filter);
    }
}