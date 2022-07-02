using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace clickfly.ViewModels
{
    public class ForgotPasswordParams
    {
        public string customer_name { get; set; }
        public string reset_password_url { get; set; }
    }
}
