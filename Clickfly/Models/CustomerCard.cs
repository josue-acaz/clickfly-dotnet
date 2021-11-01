using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("customer_cards")]
    public class CustomerCard : BaseEntity
    {
        public string card_id { get; set; }
        public string customer_id { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }

        [NotMapped]
        public string brand { get; set; }

        [NotMapped]
        public string holder_name { get; set; }

        [NotMapped]
        public string card_number { get; set; }

        [NotMapped]
        public string exp_date { get; set; }
    }
}
