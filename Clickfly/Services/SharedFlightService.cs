using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using System.Collections.Generic;
using System.Dynamic;

namespace clickfly.Services
{
    public class SharedFlightService : ISharedFlightService
    {
        private readonly ISharedFlightRepository _sharedFlightRepository;

        public SharedFlightService(ISharedFlightRepository sharedFlightRepository)
        {
            _sharedFlightRepository = sharedFlightRepository;
        }

        public async Task<SharedFlightOverviewResult> Overview(PaginationFilter filter)
        {
            SharedFlightOverviewResult sharedFlights = await _sharedFlightRepository.Overview(filter);
            return sharedFlights;
        }

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            PaginationResult<FlightSegment> paginationResult = await _sharedFlightRepository.Pagination(filter);
            return paginationResult;
        }
    }
}