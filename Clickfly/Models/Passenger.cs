using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("passengers")]
    public class Passenger : BaseEntity
    {
        public string name { get; set; }

        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
        public string email { get; set; }
        public DateTime? birthdate { get; set; }
        public string document { get; set; }
        public string document_type { get; set; }
        public string phone_number { get; set; }
        public string emergency_phone_number { get; set; }
        public string booking_id { get; set; }

        [ForeignKey("booking_id")]
        public Booking booking { get; set; }

        public string flight_segment_id { get; set; }

        [ForeignKey("flight_segment_id")]
        public FlightSegment flight_segment { get; set; }
        public Ticket ticket { get; set; }
    }
}
