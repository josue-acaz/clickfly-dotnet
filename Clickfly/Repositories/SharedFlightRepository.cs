using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Npgsql;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Text.Json;

namespace clickfly.Repositories
{
    public class SharedFlightRepository : BaseRepository<Flight>, ISharedFlightRepository
    {
        private readonly IOrm _orm;
        private static string fromSql = $"flight_segments AS flight_segment";
        private static string innerJoinFlightsSql = "flights AS flight ON flight_segment.flight_id = flight.id";
        private static string countSql = $@"
            (SELECT COUNT(*) FROM (
                select distinct on (origin_city.id, destination_city.id) 
                origin_city.id as origin_city_id, 
                destination_city.id as destination_city_id
                from flight_segments as segment
                inner join aerodromes as origin_aerodrome on origin_aerodrome.id=segment.origin_aerodrome_id
                inner join cities as origin_city on origin_city.id=origin_aerodrome.city_id
                inner join aerodromes as destination_aerodrome on destination_aerodrome.id=segment.destination_aerodrome_id
                inner join cities as destination_city on destination_city.id=destination_aerodrome.city_id
                where segment.excluded=false and segment.type='trip'
            ) as overview) as total_records
        ";

        private static string originCitySql = $@"
            (SELECT json_build_object(
                'id', origin_city.id,
                'name', origin_city.name,
                'state_id', origin_city.state_id,
                'full_name', origin_city.full_name
            ) FROM (
                SELECT city.id, city.name, city.state_id, concat(city.name, ' • ', state.prefix) as full_name FROM cities as city 
                inner join states as state on state.id=city.state_id 
                where city.excluded=false and city.id=overview.origin_city_id
            ) as origin_city) as origin_city
        ";

        private static string destinationCitySql = $@"
            (SELECT json_build_object(
                'id', destination_city.id,
                'name', destination_city.name,
                'state_id', destination_city.state_id,
                'full_name', destination_city.full_name
            ) FROM (
                SELECT city.id, city.name, city.state_id, concat(city.name, ' • ', state.prefix) as full_name FROM cities as city 
                inner join states as state on state.id=city.state_id 
                where city.excluded=false and city.id=overview.destination_city_id
            ) as destination_city) as destination_city
        ";

        private static string originAerodromeSql = $@"
            (SELECT json_build_object(
                'id', origin_aerodrome.id,
                'name', origin_aerodrome.name,
                'oaci_code', origin_aerodrome.oaci_code,
                'ciad', origin_aerodrome.ciad
            ) FROM (
                SELECT aerodrome.id, aerodrome.name, aerodrome.oaci_code, aerodrome.ciad FROM aerodromes as aerodrome 
                where aerodrome.excluded=false and aerodrome.id=flight_segment.origin_aerodrome_id
            ) as origin_aerodrome) as origin_aerodrome
        ";

        private static string destinationAerodromeSql = $@"
            (SELECT json_build_object(
                'id', destination_aerodrome.id,
                'name', destination_aerodrome.name,
                'oaci_code', destination_aerodrome.oaci_code,
                'ciad', destination_aerodrome.ciad
            ) FROM (
                SELECT aerodrome.id, aerodrome.name, aerodrome.oaci_code, aerodrome.ciad FROM aerodromes as aerodrome 
                where aerodrome.excluded=false and aerodrome.id=flight_segment.destination_aerodrome_id
            ) as destination_aerodrome) as destination_aerodrome  
        ";

        private static string flightsSql = $@"
            SELECT overview.origin_city_id, overview.destination_city_id, (SELECT json_agg(t.*) FROM (
                SELECT * FROM (
                    select 
                    flight_segment.id, 
                    flight_segment.flight_id, 
                    flight_segment.aircraft_id, 
                    flight_segment.origin_aerodrome_id, 
                    flight_segment.destination_aerodrome_id, 
                    flight_segment.number, 
                    flight_segment.departure_datetime, 
                    flight_segment.arrival_datetime, 
                    flight_segment.price_per_seat, 
                    flight_segment.total_seats, 
                    flight_segment.type,
                    {originAerodromeSql},
                    {destinationAerodromeSql}
                        from flight_segments as flight_segment 
                            inner join flights as flight on flight.id=flight_segment.flight_id
                            inner join aerodromes as origin_aerodrome on origin_aerodrome.id=flight_segment.origin_aerodrome_id
                            inner join cities as origin_city on origin_city.id=origin_aerodrome.city_id
                            inner join aerodromes as destination_aerodrome on destination_aerodrome.id=flight_segment.destination_aerodrome_id
                            inner join cities as destination_city on destination_city.id=destination_aerodrome.city_id
                            where origin_city.id=overview.origin_city_id 
                                and destination_city.id=overview.destination_city_id
                                and flight.type='shared' and flight_segment.type='trip'
                                and flight_segment.excluded=false
                                and flight_segment.type='trip' 
                                and flight_segment.departure_datetime > current_timestamp + (120 * interval '1 minute')
                ) as segments
            ) AS t) AS flights
        ";

        private static string bookedSeatsSql = $@"
            SELECT COALESCE(count(passenger.id), 0) AS  booked_seats 
            FROM passengers AS passenger 
            INNER JOIN bookings AS booking ON booking.id = passenger.booking_id 
            INNER JOIN ( SELECT status.type, status.booking_id FROM booking_status AS status WHERE status.excluded = false ORDER BY status.created_at DESC LIMIT 1 ) AS status ON status.booking_id = booking.id
            WHERE passenger.excluded = false AND booking.excluded = false AND passenger.flight_segment_id = flight_segment.id AND ( status.type::text = 'approved' OR status.type::text = 'pending' )
        ";

        private static string availableSeatsSql = $"(SELECT CAST(((flight_segment.total_seats - ({bookedSeatsSql}))) AS INTEGER))";

        private static string flightTimeSql = $"(SELECT EXTRACT(EPOCH FROM (flight_segment.arrival_datetime - flight_segment.departure_datetime)))";

        private static string subtotalSql = $"(SELECT flight_segment.price_per_seat * @selected_seats)";

        private static string whereSql = $"flight_segment.excluded = false";

        public SharedFlightRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils, IOrm orm) : base(dBContext, dataContext, utils)
        {
            _orm = orm;
        }

        public async Task<SharedFlightOverviewResult> Overview(PaginationFilter filter)
        {
            string now = _utils.DateTimeToSql(DateTime.Now);

            fromSql = $@"
                (
                    select distinct on (origin_city.id, destination_city.id) 
                    origin_city.id as origin_city_id, 
                    destination_city.id as destination_city_id
                    from flight_segments as segment
                    inner join flights as flight on segment.flight_id = flight.id
                    inner join aerodromes as origin_aerodrome on origin_aerodrome.id=segment.origin_aerodrome_id
                    inner join cities as origin_city on origin_city.id=origin_aerodrome.city_id
                    inner join aerodromes as destination_aerodrome on destination_aerodrome.id=segment.destination_aerodrome_id
                    inner join cities as destination_city on destination_city.id=destination_aerodrome.city_id
                    where segment.excluded=false 
                    and flight.type = 'shared'
                    and segment.type='trip' 
                    and segment.departure_datetime > '{now}'::date + (120 * interval '1 minute')
                ) as overview
            ";

            string querySql = $@"SELECT {countSql}, COALESCE((SELECT json_agg(json_build_object(
                'origin_city', t.origin_city,
                'destination_city', t.destination_city,
                'flights', t.flights
            )) FROM (
                {flightsSql}, 
                {originCitySql}, 
                {destinationCitySql} FROM {fromSql} ORDER BY overview.origin_city_id OFFSET 0 LIMIT 10) AS t), '[]') AS data";
            
            IEnumerable<dynamic> result = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, null, _dBContext.GetTransaction());
            dynamic[] resultArr = result.ToArray();

            string data = resultArr[0].data;
            long total_records = resultArr[0].total_records;

            List<SharedFlight> sharedFlights = JsonSerializer.Deserialize<List<SharedFlight>>(data);
            
            SharedFlightOverviewResult overviewResult = new SharedFlightOverviewResult();
            overviewResult.total_records = total_records;
            overviewResult.data = sharedFlights;

            return overviewResult;
        }

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            int selected_seats = filter.selected_seats;
            string now = _utils.DateTimeToSql(DateTime.Now);

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("selected_seats", selected_seats);

            QueryAsyncParams queryAsyncParams = new QueryAsyncParams();

            queryAsyncParams.tableName = "flight_segments";
            queryAsyncParams.relationshipName = "flight_segment";
            queryAsyncParams.querySql = $@"
                SELECT 
                    flight_segment.id, 
                    flight_segment.number, 
                    flight_segment.departure_datetime,
                    flight_segment.arrival_datetime,
                    flight_segment.price_per_seat,
                    flight_segment.total_seats,
                    flight_segment.type,
                    flight_segment.flight_id,
                    flight_segment.aircraft_id,
                    flight_segment.origin_aerodrome_id,
                    flight_segment.destination_aerodrome_id,
                    {subtotalSql} AS subtotal,
                    {flightTimeSql} AS flight_time,
                    {availableSeatsSql} AS available_seats 
                ? flight_segments AS flight_segment 
                INNER JOIN flights as flight ON flight_segment.flight_id = flight.id
                WHERE flight_segment.excluded = false 
                AND flight.type = 'shared'
                AND flight_segment.type = 'trip' 
                AND ({availableSeatsSql} - @selected_seats) >= 0 
                AND flight_segment.departure_datetime > '{now}'::date + (120 * interval '1 minute')
                LIMIT @limit OFFSET @offset
            ";

            queryAsyncParams.queryParams = queryParams;
            queryAsyncParams.foreignKey = "flight_segment_id";

            Include includeFlight = new Include();
            string[] flightAttributes = { "id", "type" };

            includeFlight.attributes = flightAttributes;
            includeFlight.tableName = "flights";
            includeFlight.foreignKey = "flight_id";
            includeFlight.relationshipName = "flight";

            Include includeAircraft = new Include();
            string[] aircraftAttributes = { "id", "prefix" };

            includeAircraft.attributes = aircraftAttributes;
            includeAircraft.tableName = "aircrafts";
            includeAircraft.foreignKey = "aircraft_id";
            includeAircraft.relationshipName = "aircraft";

            Include includeAircraftModel = new Include();
            string[] aircraftModelAttributes = { "id", "name" };

            includeAircraftModel.attributes = aircraftModelAttributes;
            includeAircraftModel.relationshipName = "model";
            includeAircraftModel.tableName = "aircraft_models";
            includeAircraftModel.foreignKey = "aircraft_model_id";
            includeAircraft.includes.Add(includeAircraftModel);
    
            string[] stateAttributes = { "id", "name", "prefix" };
            string[] cityAttributes = { "id", "name", "state_id" };
            string[] aerodromeAttributes = { "id", "name", "oaci_code" };

            Include includeState = new Include();
            includeState.attributes = stateAttributes;
            includeState.tableName = "states";
            includeState.foreignKey = "state_id";
            includeState.relationshipName = "state";

            Include includeCity = new Include();
            includeCity.attributes = cityAttributes;
            includeCity.tableName = "cities";
            includeCity.foreignKey = "city_id";
            includeCity.relationshipName = "city";
            includeCity.includes.Add(includeState);

            Include includeOriginAerodrome = new Include();
            includeOriginAerodrome.attributes = aerodromeAttributes;
            includeOriginAerodrome.tableName = "aerodromes";
            includeOriginAerodrome.foreignKey = "origin_aerodrome_id";
            includeOriginAerodrome.relationshipName = "origin_aerodrome";
            includeOriginAerodrome.includes.Add(includeCity);

            Include includeDestinationAerodrome = new Include();
            includeDestinationAerodrome.attributes = aerodromeAttributes;
            includeDestinationAerodrome.relationshipName = "destination_aerodrome";
            includeDestinationAerodrome.tableName = "aerodromes";
            includeDestinationAerodrome.foreignKey = "destination_aerodrome_id";
            includeDestinationAerodrome.includes.Add(includeCity);

            queryAsyncParams.includes.Add(includeFlight);
            queryAsyncParams.includes.Add(includeAircraft);
            queryAsyncParams.includes.Add(includeOriginAerodrome);
            queryAsyncParams.includes.Add(includeDestinationAerodrome);

            IEnumerable<FlightSegment> flightSegments = await _orm.QueryAsync<FlightSegment>(queryAsyncParams);
            
            int total_records = await _dataContext.FlightSegments.CountAsync();
            PaginationResult<FlightSegment> paginationResult = _utils.CreatePaginationResult<FlightSegment>(flightSegments.ToList(), filter, total_records);

            return paginationResult;
        }
    }
}