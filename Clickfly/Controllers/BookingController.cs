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

namespace clickfly.Controllers
{
    [Authorize]
    [Route("/bookings")]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingController(
            IDataContext dataContext, 
            IInformer informer, 
            INotificator notificator,
            IBookingService bookingService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Booking booking)
        {
            Console.WriteLine("aaaaaaa");
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
                using var transaction = _dataContext.Database.BeginTransaction();

                booking = await _bookingService.Save(booking);
                await transaction.CommitAsync();

                return HttpResponse(booking);
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
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
           
                PaginationResult<Booking> bookings = await _bookingService.Pagination(filter);
                return HttpResponse(bookings);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _bookingService.Delete(id);
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
