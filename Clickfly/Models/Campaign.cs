using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("campaigns")]
    public class Campaign : BaseEntity
    {
        public int points_to_complete { get; set; }
        public decimal discount_on_the_next_flight { get; set; }
        public DateTime expires_at { get; set; }
        public string description { get; set; }

        [NotMapped]
        public bool completed { get; set; }
    }
}
