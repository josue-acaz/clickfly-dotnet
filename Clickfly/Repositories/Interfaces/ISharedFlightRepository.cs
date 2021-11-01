using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;
using System.Dynamic;

namespace clickfly.Repositories
{
    public interface ISharedFlightRepository
    {
        Task<SharedFlightOverviewResult> Overview(PaginationFilter filter);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
    }
}