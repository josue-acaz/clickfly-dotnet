using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("tickets")]
    public class Ticket : BaseEntity
    {
        public string qr_code { get; set; }

        [ForeignKey("passenger_id")]
        public Passenger passenger { get; set; }

        [ForeignKey("flight_segment_id")]
        public FlightSegment flight_segment { get; set; }
    }
}
