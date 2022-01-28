using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PagarmeCoreApi.Standard.Models;

namespace clickfly.Models
{
    [Table("bookings")]
    public class Booking : BaseEntity
    {
        public string customer_id { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }

        public string flight_segment_id { get; set; }

        [ForeignKey("flight_segment_id")]
        public FlightSegment flight_segment { get; set; }
        public List<BookingStatus> status { get; set; }
        public List<BookingPayment> payments { get; set; }
        public List<Passenger> passengers { get; set; }

        [NotMapped]
        public string customer_card_id { get; set; }

        [NotMapped]
        public string customer_address_id { get; set; }

        [NotMapped]
        public string[] selected_passengers { get; set; }

        [NotMapped]
        public int installments { get; set; }

        [NotMapped]
        public string payment_method { get; set; }

        [NotMapped]
        public bool customer_is_passenger { get; set; }

        [NotMapped]
        public GetTransactionResponse payment_transaction { get; set; }
        
        public Booking()
        {
            status = new List<BookingStatus>();
        }
    }
}
