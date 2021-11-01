using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("booking_payments")]
    public class BookingPayment : BaseEntity
    {
        public string order_id { get; set; }
        public string payment_method { get; set; }
        public string booking_id { get; set; }

        [ForeignKey("booking_id")]
        public Booking booking { get; set; }
    }
}
