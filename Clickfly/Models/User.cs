using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string name { get; set; }

        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
        public string email { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string role { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public virtual string password { get; set; }
        public string password_hash { get; set; }
        public string air_taxi_id { get; set; }

        [ForeignKey("air_taxi_id")]
        public AirTaxi air_taxi { get; set; }
    }
}
