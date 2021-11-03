using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IAppFlightRepository
    {
        Task<PaginationResult<AppFlight>> Overview(PaginationFilter filter);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
    }
}
