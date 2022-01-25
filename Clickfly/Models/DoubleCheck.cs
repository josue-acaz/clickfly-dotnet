using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("double_checks")]
    public class DoubleCheck : BaseEntity
    {
        public bool? approved { get; set; }
        public string message { get; set; }
        public string user_id { get; set; }
        public string approver_id { get; set; }
        public string resource_id { get; set; }
        public string resource { get; set; }
    }
}
