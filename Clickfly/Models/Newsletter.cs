using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("newsletters")]
    public class Newsletter : BaseEntity
    {
        public string code { get; set; }
        public string details { get; set; }
    }
}
