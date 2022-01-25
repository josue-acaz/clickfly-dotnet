using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("flight_segments")]
    public class FlightSegment : BaseEntity
    {
        public int number { get; set; }
        public DateTime departure_datetime { get; set; }
        public DateTime arrival_datetime { get; set; }
        public decimal price_per_seat { get; set; }
        public int total_seats { get; set; }
        public string type { get; set; }
        public decimal distance { get; set; }
        public decimal flight_time { get; set; }
        public string flight_id { get; set; }

        [ForeignKey("flight_id")]
        public Flight flight { get; set; }
        public string aircraft_id { get; set; }

        [ForeignKey("aircraft_id")]
        public Aircraft aircraft { get; set; }
        public string origin_aerodrome_id { get; set; }

        [ForeignKey("origin_aerodrome_id")]
        public Aerodrome origin_aerodrome { get; set; }
        public string destination_aerodrome_id { get; set; }

        [ForeignKey("destination_aerodrome_id")]
        public Aerodrome destination_aerodrome { get; set; }

        [NotMapped]
        public double subtotal { get; set; }

        [NotMapped]
        public int available_seats { get; set; }

        [NotMapped]
        public string status { get; set; }
    }
}
