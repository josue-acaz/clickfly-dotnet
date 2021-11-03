using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("customers")]
    public class Customer : BaseEntity
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string name { get; set; }

        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
        public string email { get; set; }
        public string phone_number { get; set; }
        public string emergency_phone_number { get; set; }
        public string document { get; set; }
        public string document_type { get; set; }

        [NotMapped]
        public virtual string password { get; set; }
        public string password_hash { get; set; }
        public DateTime? password_reset_expires { get; set; }
        public string password_reset_token { get; set; }
        public string role { get; set; }
        public string type { get; set; }
        public DateTime? birthdate { get; set; }
        public bool verified { get; set; }
        public string customer_id { get; set; }
        public List<CustomerAddress> addresses { get; set; }
        public List<CustomerCard> cards { get; set; }
        public List<CustomerFriend> friends { get; set; }

        [NotMapped]
        public string thumbnail { get; set; }
    }
}
