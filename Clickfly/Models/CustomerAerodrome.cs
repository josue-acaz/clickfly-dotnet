using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("customer_aerodromes")]
    public class CustomerAerodrome : BaseEntity
    {
        public string customer_id { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }

        public string aerodrome_id { get; set; }

        [ForeignKey("aerodrome_id")]
        public Aerodrome aerodrome { get; set; }
    }
}
