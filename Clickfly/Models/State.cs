using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("states")]
    public class State : BaseEntity
    {
        public string name { get; set; }
        public string prefix { get; set; }
    }
}
