using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("access_tokens")]
    public class AccessToken : BaseEntity
    {
        public string token { get; set; }
        public string resource_id { get; set; }
        public DateTime? expires_in { get; set; }
    }
}
