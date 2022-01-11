using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IAircraftModelRepository
    {
        Task<AircraftModel> Create(AircraftModel aircraftModel);
        Task<AircraftModel> GetById(string id);
        Task<AircraftModel> Update(AircraftModel aircraftModel);
        Task Delete(string id);
        Task<PaginationResult<AircraftModel>> Pagination(PaginationFilter filter);
    }
}
