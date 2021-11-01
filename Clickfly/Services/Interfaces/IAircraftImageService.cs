using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface IAircraftImageService
    {
        Task<AircraftImage> Save(AircraftImage aircraftImage);
        Task Delete(string id);
        Task<PaginationResult<AircraftImage>> Pagination(AircraftImagePaginationFilter filter);
    }
}
