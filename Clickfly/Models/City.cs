using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("cities")]
    public class City : BaseEntity
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string name { get; set; }
        public string prefix { get; set; }
        public string description { get; set; }
        public string state_id { get; set; }

        [ForeignKey("state_id")]
        public State state { get; set; }
        public string timezone_id { get; set; }

        [ForeignKey("timezone_id")]
        public Timezone timezone { get; set; }

        [NotMapped]
        public int gmt { get; set; }

        [NotMapped]
        public string state_prefix { get; set; }

        [NotMapped]
        public string full_name { get { return $"{name} • {state?.prefix}"; } }
    }
}
