using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("permissions")]
    public class Permission : BaseEntity
    {
        public bool create { get; set; }
        public bool read { get; set; }
        public bool update { get; set; }
        public bool delete { get; set; }
        public string permission_group_id { get; set; }

        [ForeignKey("permission_group_id")]
        public PermissionGroup permissionGroup { get; set; }

        public string permission_resource_id { get; set; }

        [ForeignKey("permission_resource_id")]
        public PermissionResource permissionResource { get; set; }
    }
}
