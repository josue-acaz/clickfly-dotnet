using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using clickfly.ViewModels;
using System.Dynamic;

namespace clickfly.Controllers
{
    [Route("/shared-flights")]
    public class SharedFlightController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly ISharedFlightService _sharedFlightService;

        public SharedFlightController(IDataContext dataContext, ISharedFlightService sharedFlightService)
        {
            _dataContext  = dataContext;
            _sharedFlightService = sharedFlightService;
        }

        [HttpGet("overview")]
        [AllowAnonymous]
        public async Task<ActionResult> Overview([FromQuery]PaginationFilter filter)
        {
            SharedFlightOverviewResult sharedFlights = await _sharedFlightService.Overview(filter);
            return HttpResponse(sharedFlights);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<FlightSegment> flightSegments = await _sharedFlightService.Pagination(filter);
            return HttpResponse(flightSegments);
        }
    }
}
