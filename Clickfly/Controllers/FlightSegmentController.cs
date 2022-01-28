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
    [Authorize]
    [Route("/flight-segments")]
    public class FlightSegmentController : BaseController
    {
        private readonly IFlightSegmentService _flightSegmentService;

        public FlightSegmentController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IFlightSegmentService flightSegmentService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _flightSegmentService = flightSegmentService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<FlightSegment>> GetById(string id)
        {
            try
            {
                FlightSegment flightSegment = await _flightSegmentService.GetById(id);
                return HttpResponse(flightSegment);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost]
        [Authorize(Roles = "employee,manager,administrator")]
        public async Task<ActionResult> Save([FromBody]FlightSegment flightSegment)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                FlightSegment _flightSegment = await _flightSegmentService.Save(flightSegment);
                await transaction.CommitAsync();

                // Enviar notificação de novo voo
                //await _flightSegmentService.SendNotification(_flightSegment.id);

                return HttpResponse(_flightSegment);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet]
        [Authorize(Roles = "employee,manager")]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            try
            {
                PaginationResult<FlightSegment> flightSegments = await _flightSegmentService.Pagination(filter);
                return HttpResponse(flightSegments);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "employee,manager")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _flightSegmentService.Delete(id);
                return HttpResponse();
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }
    }
}
