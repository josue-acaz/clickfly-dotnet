using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("aircrafts")]
    public class Aircraft : BaseEntity
    {
        public string prefix { get; set; }
        public int year { get; set; }
        public int crew { get; set; }
        public int passengers { get; set; }
        public float empty_weight { get; set; }
        public float autonomy { get; set; }
        public float maximum_takeoff_weight { get; set; }
        public float maximum_speed { get; set; }
        public float cruising_speed { get; set; }
        public float range { get; set; }
        public float fixed_price { get; set; }
        public float fixed_price_radius { get; set; }
        public float price_per_km { get; set; }
        public string description { get; set; }
        public bool pressurized { get; set; }
        public string aircraft_model_id { get; set; }

        [ForeignKey("aircraft_model_id")]
        public AircraftModel model { get; set; }
        public string air_taxi_id { get; set; }

        [ForeignKey("air_taxi_id")]
        public AirTaxi air_taxi { get; set; }
        public List<Flight> flights { get; set; }

        [NotMapped]
        public string full_name { get { return $"{prefix} • {model?.name}"; } }
    }
}
