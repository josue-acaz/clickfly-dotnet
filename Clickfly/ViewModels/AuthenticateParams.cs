using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace clickfly.ViewModels
{
    public class AuthenticateParams
    {
        public string username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string password { get; set; }
    }
}
