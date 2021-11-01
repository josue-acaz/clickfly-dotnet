using System;

namespace clickfly.ViewModels
{
    public class DistanceBetweenAerodromesRequest
    {
        public string origin_aerodrome_id { get; set; }
        public string destination_aerodrome_id { get; set; }
    }
}
