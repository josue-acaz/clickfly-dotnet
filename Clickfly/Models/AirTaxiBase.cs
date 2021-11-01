using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("air_taxi_bases")]
    public class AirTaxiBase : BaseEntity
    {
        public string name { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string air_taxi_id { get; set; }

        [ForeignKey("air_taxi_id")]
        public AirTaxi air_taxi { get; set; }
        public string aerodrome_id { get; set; }

        [ForeignKey("aerodrome_id")]
        public Aerodrome aerodrome { get; set; }
    }
}
