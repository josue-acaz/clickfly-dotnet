using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using clickfly.Models;

namespace clickfly.ViewModels
{
    public class AppFlight
    {
        public City origin_city { get; set; }
        public City destination_city { get; set; }
        public List<FlightSegment> flights { get; set; }
    }
}
