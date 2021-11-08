using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace clickfly.Repositories
{
    public class FlightSegmentRepository : BaseRepository<FlightSegment>, IFlightSegmentRepository
    {
        private readonly IOrm _orm;
        private static string fieldsSql = "*";
        private static string fromSql = "flight_segments as flight_segment";
        private static string whereSql = "flight_segment.excluded = false";
        private static string bookedSeatsSql = $@"
            SELECT COALESCE(count(passenger.id), 0) AS  booked_seats FROM passengers AS passenger 
            INNER JOIN bookings AS booking ON booking.id = passenger.booking_id 
            INNER JOIN ( SELECT status.type, status.booking_id FROM booking_status AS status WHERE status.excluded = false ORDER BY status.created_at DESC LIMIT 1 ) AS status ON status.booking_id = booking.id
            WHERE passenger.excluded = false AND booking.excluded = false AND passenger.flight_segment_id = flight_segment.id AND ( status.type::text = 'approved' OR status.type::text = 'pending' )
        ";

        private static string availableSeatsSql = $"(SELECT CAST(((flight_segment.total_seats - ({bookedSeatsSql}))) AS INTEGER))";

        private static string flightTimeSql = $"(SELECT EXTRACT(EPOCH FROM (flight_segment.arrival_datetime - flight_segment.departure_datetime)))";
        protected string[] defaultFields = new string[10];

        public FlightSegmentRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils, IOrm orm) : base(dBContext, dataContext, utils)
        {
            _orm = orm;
            defaultFields[0] = "number";
            defaultFields[1] = "departure_datetime";
            defaultFields[2] = "arrival_datetime";
            defaultFields[3] = "price_per_seat";
            defaultFields[4] = "total_seats";
            defaultFields[5] = "type";
            defaultFields[6] = "flight_id";
            defaultFields[7] = "aircraft_id";
            defaultFields[8] = "origin_aerodrome_id";
            defaultFields[9] = "destination_aerodrome_id";
        }

        public async Task<FlightSegment> Create(FlightSegment flightSegment)
        {
            string id = Guid.NewGuid().ToString();
            flightSegment.id = id;

            await _dataContext.FlightSegments.AddAsync(flightSegment);
            await _dataContext.SaveChangesAsync();

            return flightSegment;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<FlightSegment> GetById(string id)
        {
            Console.WriteLine("PK" + id);
            string querySql = $@"SELECT 
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
                {flightTimeSql} AS flight_time,
                {availableSeatsSql} AS available_seats 
            ? {fromSql} WHERE {whereSql} AND flight_segment.id = @id";
            
            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("id", id);

            QueryAsyncParams queryAsyncParams = new QueryAsyncParams();
            queryAsyncParams.querySql = querySql;
            queryAsyncParams.queryParams = queryParams;
            queryAsyncParams.relationshipName = "flight_segment";
            queryAsyncParams.tableName = "flight_segments";
            queryAsyncParams.foreignKey = "flight_segment_id";

            Include includeFlight = new Include();
            string[] flightAttributes = { "id", "type" };

            includeFlight.attributes = flightAttributes;
            includeFlight.tableName = "flights";
            includeFlight.foreignKey = "flight_id";
            includeFlight.relationshipName = "flight";

            Include includeAircraft = new Include();
            string[] aircraftAttributes = { "id", "prefix" };

            RawAttribute[] aircraftRawAttributes = new RawAttribute[1];
            RawAttribute aircraftThumbnail = new RawAttribute();
            aircraftThumbnail.name = "thumbnail";
            aircraftThumbnail.query = "SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";
            aircraftRawAttributes[0] = aircraftThumbnail;

            includeAircraft.tableName = "aircrafts";
            includeAircraft.foreignKey = "aircraft_id";
            includeAircraft.relationshipName = "aircraft";
            includeAircraft.attributes = aircraftAttributes;
            includeAircraft.rawAttributes = aircraftRawAttributes;

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
            
            FlightSegment flightSegment = await _orm.QuerySingleOrDefaultAsync<FlightSegment>(queryAsyncParams);
            
            return flightSegment;
        }

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            string flightId = filter.flight_id;

            List<FlightSegment> flightSegments = await _dataContext.FlightSegments
                .Include(flightSegment => flightSegment.aircraft)
                .ThenInclude(aircraft => aircraft.model)
                .Include(flightSegment => flightSegment.origin_aerodrome)
                .Include(flightSegment => flightSegment.destination_aerodrome)
                .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
                .Take(paginationFilter.page_size)
                .Where(flightSegment => flightSegment.excluded == false && flightSegment.flight_id == flightId)
                .ToListAsync();
            
            int total_records = await _dataContext.Flights.CountAsync();
            PaginationResult<FlightSegment> paginationResult = _utils.CreatePaginationResult<FlightSegment>(flightSegments, paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<FlightSegment> Update(FlightSegment flightSegment, string[] fields = null)
        {
            if(fields == null)
            {
                fields = defaultFields;
            }

            GetFieldsSqlParams fieldsSqlParams = new GetFieldsSqlParams();
            fieldsSqlParams.action = "UPDATE";
            fieldsSqlParams.fields = fields;

            string fieldsToUpdate = _utils.GetFieldsSql(fieldsSqlParams);
            string querySql = $"UPDATE flight_segments SET {fieldsToUpdate} WHERE id = @id";

            await _dBContext.GetConnection().ExecuteAsync(querySql, flightSegment, _dBContext.GetTransaction());

            return flightSegment;
        }
    }
}
