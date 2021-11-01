using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;
using System.Collections.Generic;
using System.Dynamic;

namespace clickfly.Services
{
    public interface ISharedFlightService
    {
        Task<SharedFlightOverviewResult> Overview(PaginationFilter filter);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
    }
}