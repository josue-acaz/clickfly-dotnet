using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("push_notifications")]
    public class PushNotification : BaseEntity
    {
        public string flight_segment_id { get; set; }

        [ForeignKey("flight_segment_id")]
        FlightSegment flightSegment { get; set; }
        public string notification_id { get; set; }
    }
}
