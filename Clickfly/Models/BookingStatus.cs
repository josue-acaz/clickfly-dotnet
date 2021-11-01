using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("booking_status")]
    public class BookingStatus : BaseEntity
    {
        public string type { get; set; }
        public string booking_id { get; set; }

        [ForeignKey("booking_id")]
        public Booking booking { get; set; }
    }
}
