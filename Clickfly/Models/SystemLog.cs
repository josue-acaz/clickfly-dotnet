using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("system_logs")]
    public class SystemLog : BaseEntity
    {
        public string ip { get; set; }
        public string action { get; set; }
        public string resource { get; set; }
        public string resource_id { get; set; }
        public string user_id { get; set; }
        public string user_type { get; set; }
        public string _object { get; set; }
    }
}
