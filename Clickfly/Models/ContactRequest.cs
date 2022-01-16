using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("contact_requests")]
    public class ContactRequest : BaseEntity
    {
        public string name { get; set; }
        public string subject { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string message { get; set; }
        public bool read { get; set; } 
    }
}
