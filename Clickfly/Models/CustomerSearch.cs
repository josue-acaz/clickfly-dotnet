using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("customer_searches")]
    public class CustomerSearch : BaseEntity
    {
        public string origin_id { get; set; }
        public string origin_type { get; set; }
        public string destination_id { get; set; }
        public string destination_type { get; set; }
        public DateTime? departure_datetime { get; set; }
        public DateTime? arrival_datetime { get; set; }
        public int seats { get; set; }
        public string customer_id { get; set; }
    }
}
