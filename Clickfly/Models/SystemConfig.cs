using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("system_configs")]
    public class SystemConfig : BaseEntity
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
