using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace clickfly.ViewModels
{
    public class ThumbnailRequest
    {
        public IFormFile file { get; set; }

        // for aircraft
        public string type { get; set; }
        public string aircraft_id { get; set; }

        // for customers
        public string customer_id { get; set; }
    }
}
