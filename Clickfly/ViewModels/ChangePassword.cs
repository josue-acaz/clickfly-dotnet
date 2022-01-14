using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace clickfly.ViewModels
{
    public class ChangePassword
    {
        public string actual_password { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }
}
