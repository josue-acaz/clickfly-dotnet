using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IAircraftImageService
    {
        Task<AircraftImage> Save(AircraftImage aircraftImage);
        Task Delete(string id);
        Task<PaginationResult<AircraftImage>> Pagination(AircraftImagePaginationFilter filter);
    }
}
