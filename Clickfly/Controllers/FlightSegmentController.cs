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
    [Route("/flight-segments")]
    public class FlightSegmentController : BaseController
    {
        private readonly IFlightSegmentService _flightSegmentService;

        public FlightSegmentController(IDataContext dataContext, IInformer informer, IFlightSegmentService flightSegmentService) : base(dataContext, informer)
        {
            _flightSegmentService = flightSegmentService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<FlightSegment>> GetById(string id)
        {
            FlightSegment flightSegment = await _flightSegmentService.GetById(id);
            return HttpResponse(flightSegment);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]FlightSegment flightSegment)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            FlightSegment _flightSegment = await _flightSegmentService.Save(flightSegment);
            await transaction.CommitAsync();

            return HttpResponse(_flightSegment);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<FlightSegment> flightSegments = await _flightSegmentService.Pagination(filter);
            return HttpResponse(flightSegments);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _flightSegmentService.Delete(id);
            return HttpResponse();
        }
    }
}
