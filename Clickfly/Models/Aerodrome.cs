using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("aerodromes")]
    public class Aerodrome : BaseEntity
    {
        public string oaci_code { get; set; }
        public string ciad { get; set; }
        public string name { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public float altitude { get; set; }
        public float length { get; set; }
        public float width { get; set; }
        public string operation { get; set; }
        public string designation { get; set; }
        public string resistance { get; set; }
        public string surface { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string access { get; set; }
        public string city_id { get; set; }

        [ForeignKey("city_id")]
        public City city { get; set; }

        [NotMapped]
        public string city_name { get; set; }

        [NotMapped]
        public string state_prefix { get; set; }

        [NotMapped]
        public string full_name { get { return $"{oaci_code} • {name}"; } }
    }
}
