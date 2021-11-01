using System;
using System.Collections.Generic;
using clickfly.Models;

namespace clickfly.ViewModels
{
    public class Authenticated
    {
        public string token { get; set; }
        public dynamic auth_user { get; set; }
    }
}
