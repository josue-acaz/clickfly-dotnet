using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using System.Collections.Generic;
using System.Dynamic;

namespace clickfly.Services
{
    public class AppFlightService : IAppFlightService
    {
        private readonly IAppFlightRepository _appFlightRepository;

        public AppFlightService(IAppFlightRepository appFlightRepository)
        {
            _appFlightRepository = appFlightRepository;
        }

        public async Task<PaginationResult<AppFlight>> Overview(PaginationFilter filter)
        {
            PaginationResult<AppFlight> paginationResult = await _appFlightRepository.Overview(filter);
            return paginationResult;
        }

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            PaginationResult<FlightSegment> paginationResult = await _appFlightRepository.Pagination(filter);
            return paginationResult;
        }
    }
}