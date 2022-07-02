using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace clickfly.ViewModels
{
    public class ResetPasswordParams
    {
        public string token { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }
}
