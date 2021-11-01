using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("customer_addresses")]
    public class CustomerAddress : BaseEntity
    {
        public string name { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string zipcode { get; set; }
        public string neighborhood { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address_id { get; set; }
        public string complement { get; set; }
        public string customer_id { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }
    }
}
