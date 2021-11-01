using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace clickfly.Models
{
    [Table("aircraft_images")]
    public class AircraftImage : BaseEntity
    {
        public string view { get; set; }
        public string aircraft_id { get; set; }
        
        [ForeignKey("aircraft_id")]
        public Aircraft aircraft { get; set; }

        [NotMapped]
        public string url { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }
    }
}
