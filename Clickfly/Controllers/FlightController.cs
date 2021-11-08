using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
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

        public FlightController(IDataContext dataContext, IInformer informer, IFlightService flightService) : base(dataContext, informer)
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
            User user = _informer.GetValue<User>(UserTypes.User);
            
            using var transaction = _dataContext.Database.BeginTransaction();

            flight.air_taxi_id = user.air_taxi_id;
            flight = await _flightService.Save(flight);
            await transaction.CommitAsync();

            return HttpResponse(flight);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            User user = _informer.GetValue<User>(UserTypes.User);

            filter.air_taxi_id = user.air_taxi_id;
            PaginationResult<Flight> flights = await _flightService.Pagination(filter);
            
            return HttpResponse(flights);
        }
    }
}
