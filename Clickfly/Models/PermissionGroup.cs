using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("permission_groups")]
    public class PermissionGroup : BaseEntity
    {
        public string user_id { get; set; }

        [ForeignKey("user_id")]
        public User user { get; set; }

        public string user_role_id { get; set; }

        [ForeignKey("user_role_id")]
        public UserRole role { get; set; }
        
        public List<Permission> permissions { get; set; }
        public bool allowed { get; set; }
    }
}
