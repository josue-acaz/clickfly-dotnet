using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("account_verifications")]
    public class AccountVerification : BaseEntity
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string token { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public DateTime expires { get; set; }
        public string customer_id { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }
    }
}
