using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("user_roles")]
    public class UserRole : BaseEntity
    {
        public string name { get; set; }
        public string label { get; set; }
        public string allowed_systems { get; set; }
        public List<UserRolePermission> permissions { get; set; }
    }
}
