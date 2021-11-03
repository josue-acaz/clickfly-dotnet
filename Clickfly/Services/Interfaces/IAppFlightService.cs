using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;
using System.Collections.Generic;
using System.Dynamic;

namespace clickfly.Services
{
    public interface IAppFlightService
    {
        Task<PaginationResult<AppFlight>> Overview(PaginationFilter filter);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
    }
}