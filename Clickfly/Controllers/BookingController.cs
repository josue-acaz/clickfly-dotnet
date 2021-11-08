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
    [Authorize]
    [Route("/bookings")]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IDataContext dataContext, IInformer informer, IBookingService bookingService) : base(dataContext, informer)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Booking booking)
        {
            string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
            using var transaction = _dataContext.Database.BeginTransaction();

            booking.customer_id = customerId;
            CreateBookingResponse createBookingResponse = await _bookingService.Save(booking);
            await transaction.CommitAsync();

            return HttpResponse(createBookingResponse);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);

            filter.customer_id = customerId;            
            PaginationResult<Booking> bookings = await _bookingService.Pagination(filter);
            return HttpResponse(bookings);
        }
    }
}
