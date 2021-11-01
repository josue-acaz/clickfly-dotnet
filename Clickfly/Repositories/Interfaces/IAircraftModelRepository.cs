using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IAircraftModelRepository
    {
        Task<AircraftModel> Create(AircraftModel aircraftModel);
        Task<AircraftModel> GetById(string id);
        Task<AircraftModel> Update(AircraftModel aircraftModel, string[] fields = null);
        Task Delete(string id);
        Task<PaginationResult<AircraftModel>> Pagination(PaginationFilter filter);
    }
}
