using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Services;
using clickfly.ViewModels;
using Newtonsoft.Json;

namespace clickfly.Controllers
{
    [Authorize]
    [Route("/flights")]
    public class FlightController : BaseController
    {
        private readonly IFlightService _flightService;

        public FlightController(
            IDataContext dataContext, 
            IInformer informer, 
            INotificator notificator,
            IFlightService flightService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _flightService = flightService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetById(string id)
        {
            Flight flight = await _flightService.GetById(id);
            return HttpResponse(flight);
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Flight flight)
        {
            GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            
            using var transaction = _dataContext.Database.BeginTransaction();

            flight = await _flightService.Save(flight);
            await transaction.CommitAsync();

            return HttpResponse(flight);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            
            PaginationResult<Flight> flights = await _flightService.Pagination(filter);
            return HttpResponse(flights);
        }
    }
}
