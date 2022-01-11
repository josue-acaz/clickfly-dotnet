using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("permission_resources")]
    public class PermissionResource : BaseEntity
    {
        public string name { get; set; }
        public string _table { get; set; }
    }
}