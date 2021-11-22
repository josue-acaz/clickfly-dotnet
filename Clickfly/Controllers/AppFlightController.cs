using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using clickfly.Data;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Route("/app-flights")]
    public class AppFlightController : BaseController
    {
        private readonly IAppFlightService _appFlightService;

        public AppFlightController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IAppFlightService appFlightService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _appFlightService = appFlightService;
        }

        [HttpGet("overview")]
        [AllowAnonymous]
        public async Task<ActionResult> Overview([FromQuery]PaginationFilter filter)
        {
            PaginationResult<AppFlight> flights = await _appFlightService.Overview(filter);
            return HttpResponse(flights);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<FlightSegment> flightSegments = await _appFlightService.Pagination(filter);
            return HttpResponse(flightSegments);
        }
    }
}
