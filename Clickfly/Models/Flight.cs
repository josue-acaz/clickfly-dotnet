using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("flights")]
    public class Flight : BaseEntity
    {
        public string type { get; set; }
        public string aircraft_id { get; set; }

        [ForeignKey("aircraft_id")]
        public Aircraft aircraft { get; set; }
        public string air_taxi_id { get; set; }

        [ForeignKey("air_taxi_id")]
        public AirTaxi air_taxi { get; set; }
        public List<FlightSegment> segments { get; set; }
    }
}
