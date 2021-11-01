using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("manufacturers")]
    public class Manufacturer : BaseEntity
    {
        public string name { get; set; }
        public string country { get; set; }
        public string description { get; set; }

        IEnumerable<AircraftModel> aircraft_models { get; set; }
    }
}
