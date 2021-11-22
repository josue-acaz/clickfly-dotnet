using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("user_roles")]
    public class UserRole : BaseEntity
    {
        public string name { get; set; }
    }
}