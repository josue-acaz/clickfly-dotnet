using System;
using System.ComponentModel.DataAnnotations;

namespace clickfly.Models
{
    public class BaseEntity
    {
        [Key]
        public string id { get; set; }
        public bool excluded { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}