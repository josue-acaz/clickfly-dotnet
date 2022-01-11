using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IAppFlightRepository
    {
        Task<PaginationResult<AppFlight>> Overview(PaginationFilter filter);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
    }
}
