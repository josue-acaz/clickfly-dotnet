using System;
using clickfly.Models;
using PagarmeCoreApi.Standard.Models;

namespace clickfly.ViewModels
{
    public class CreateBookingResponse
    {
        public BookingStatus booking_status { get; set; }
        public GetTransactionResponse payment_transaction { get; set; }
    }
}
