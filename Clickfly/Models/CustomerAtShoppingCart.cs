using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("customer_at_shopping_carts")]
    public class CustomerAtShoppingCart : BaseEntity
    {
        public string flight_segment_id { get; set; }
        public string selected_seats { get; set; }
        public string payment_method { get; set; }
        public string customer_id { get; set; }
    }
}
