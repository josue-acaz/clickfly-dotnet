using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IAirTaxiRepository
    {
        Task<AirTaxi> Create(AirTaxi airTaxi);
        Task<AirTaxi> GetById(string id);
        Task<AirTaxi> Update(AirTaxi airTaxi);
        Task Delete(string id);
        Task<PaginationResult<AirTaxi>> Pagination(PaginationFilter filter);
    }
}