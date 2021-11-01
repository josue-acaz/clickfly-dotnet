using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using clickfly.Models;

namespace clickfly.ViewModels
{
    public class SharedFlight
    {
        public City origin_city { get; set; }
        public City destination_city { get; set; }
        public FlightSegment[] flights { get; set; }
    }
}
