using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Route("/flights")]
    public class FlightController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly IFlightService _flightService;

        public FlightController(IDataContext dataContext, IFlightService flightService)
        {
            _dataContext  = dataContext;
            _flightService = flightService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Flight>> GetById(string id)
        {
            Flight flight = await _flightService.GetById(id);
            return HttpResponse(flight);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]Flight flight)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            Flight _flight = await _flightService.Save(flight);
            await transaction.CommitAsync();

            return HttpResponse(_flight);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<Flight> flights = await _flightService.Pagination(filter);
            
            return HttpResponse(flights);
        }
    }
}
