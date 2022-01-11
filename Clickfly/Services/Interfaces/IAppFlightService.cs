using System;
using System.Threading.Tasks;
using clickfly.Models;
using System.Collections.Generic;
using System.Dynamic;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IAppFlightService
    {
        Task<PaginationResult<AppFlight>> Overview(PaginationFilter filter);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
    }
}