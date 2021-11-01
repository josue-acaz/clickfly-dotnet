using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("aircraft_models")]
    public class AircraftModel : BaseEntity
    {
        public string name { get; set; }
        public string type { get; set; }
        public string carrier_size { get; set; }
        public string number_of_engines { get; set; }
        public string engine_type { get; set; }
        public string carrier_dimensions { get; set; }
        public string manufacturer_id { get; set; }

        [ForeignKey("manufacturer_id")]
        public Manufacturer manufacturer { get; set; }
        public List<Aircraft> aircrafts { get; set; }
    }
}
