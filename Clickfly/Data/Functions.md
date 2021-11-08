# CREATE FUNCTIONS TO WORK WITH BOOKINGS

/*Retorna a quantidade de assentos reservados para determinado voo*/
CREATE OR REPLACE FUNCTION get_booked_seats(varchar)
RETURNS integer AS $$
BEGIN
   RETURN COALESCE(COUNT(passenger.id), 0) AS booked_seats FROM passengers AS passenger 
		INNER JOIN bookings AS booking ON passenger.booking_id = booking.id
		INNER JOIN booking_status AS status ON status.booking_id = booking.id
		WHERE booking.excluded = false AND booking.flight_segment_id = $1
		AND (status.type::text = 'approved' OR status.type::text = 'pending');
END;
$$ LANGUAGE plpgsql;
