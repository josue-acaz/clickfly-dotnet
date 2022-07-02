using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("files")]
    public class File : BaseEntity
    {
        public string resource { get; set; }
        public string resource_id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string mimetype { get; set; }
        public string field_name { get; set; }
        public long size { get; set; }
        
        [NotMapped]
        public string url { get; set; }
    }
}
