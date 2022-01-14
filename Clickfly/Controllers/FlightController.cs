using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Services;
using Newtonsoft.Json;
using clickfly.ViewModels;

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
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                Flight flight = await _flightService.GetById(id);
                
                return HttpResponse(flight);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Flight flight)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                flight = await _flightService.Save(flight);
                await transaction.CommitAsync();

                flight = await _flightService.GetById(flight.id);

                return HttpResponse(flight);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                PaginationResult<Flight> flights = await _flightService.Pagination(filter);
                
                return HttpResponse(flights);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }
    }
}
