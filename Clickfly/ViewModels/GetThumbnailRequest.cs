using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace clickfly.ViewModels
{
    public class GetThumbnailRequest
    {
        public string type { get; set; }
        public string aircraft_id { get; set; }
    }
}
