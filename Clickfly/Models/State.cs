using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("states")]
    public class State : BaseEntity
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string name { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string prefix { get; set; }
    }
}
