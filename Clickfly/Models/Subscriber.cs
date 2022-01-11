using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("subscribers")]
    public class Subscriber : BaseEntity
    {
        public string email { get; set; }
        public string details { get; set; }
        public bool unsubscribed { get; set; }
    }
}
