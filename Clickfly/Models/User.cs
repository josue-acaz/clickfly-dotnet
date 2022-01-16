using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("users")]
    public class User : BaseEntity
    {
        public string name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string phone_number { get; set; }
        public string emergency_phone_number { get; set; }

        [NotMapped]
        public string role { get; set; }

        [NotMapped]
        public virtual string password { get; set; }
        public string password_hash { get; set; }
        public string air_taxi_id { get; set; }

        [ForeignKey("air_taxi_id")]
        public AirTaxi air_taxi { get; set; }
    }
}
