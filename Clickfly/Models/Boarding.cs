using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("boardings")]
    public class Boarding : BaseEntity
    {
        public string passenger_id { get; set; }
        public string flight_segment_id { get; set; }
    }
}
