using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IAircraftImageRepository
    {
        Task<AircraftImage> Create(AircraftImage aircraftImage);
        Task<AircraftImage> GetById(string id);
        Task<AircraftImage> Update(AircraftImage aircraftImage);
        Task Delete(string id);
        Task<PaginationResult<AircraftImage>> Pagination(AircraftImagePaginationFilter filter);
    }
}
