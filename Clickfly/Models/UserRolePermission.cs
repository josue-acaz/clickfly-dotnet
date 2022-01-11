using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("user_role_permissions")]
    public class UserRolePermission : BaseEntity
    {
        public bool _create { get; set; }
        public bool _read { get; set; }
        public bool _update { get; set; }
        public bool _delete { get; set; }
        public string user_role_id { get; set; }

        [ForeignKey("user_role_id")]
        public UserRole userRole { get; set; }
        public string permission_resource_id { get; set; }

        [ForeignKey("permission_resource_id")]
        public PermissionResource permissionResource { get; set; }

        [NotMapped]
        public string permission_resource_name { get; set; }
    }
}
