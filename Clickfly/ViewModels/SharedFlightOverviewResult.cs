using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using clickfly.Models;

namespace clickfly.ViewModels
{
    public class SharedFlightOverviewResult
    {
        public long total_records { get; set; }
        public List<SharedFlight> data { get; set; }
    }
}
