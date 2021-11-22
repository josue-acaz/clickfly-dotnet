using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("flight_segment_status")]
    public class FlightSegmentStatus : BaseEntity
    {
        public string type { get; set; }
        public string annotation { get; set; }
        public string flight_segment_id { get; set; }

        [ForeignKey("flight_segment_id")]
        public FlightSegment flight_segment { get; set;}
    }
}
