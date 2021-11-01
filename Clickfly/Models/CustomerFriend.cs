using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("customer_friends")]
    public class CustomerFriend : BaseEntity
    {
        public string name { get; set; }

        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
        public string email { get; set; }
        public DateTime? birthdate { get; set; }
        public string document { get; set; }
        public string document_type { get; set; }
        public string phone_number { get; set; }
        public string emergency_phone_number { get; set; }
        public string customer_id { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }
    }
}
