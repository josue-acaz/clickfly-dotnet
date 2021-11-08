using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace clickfly.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        private readonly IOrm _orm;
        private static string fieldsSql = "*";
        private static string fromSql = "tickets as ticket";
        private static string whereSql = "ticket.excluded = false";

        public TicketRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils, IOrm orm) : base(dBContext, dataContext, utils)
        {
            _orm = orm;
        }

        public async Task<Ticket> Create(Ticket ticket)
        {
            string id = Guid.NewGuid().ToString();
            ticket.id = id;

            await _dataContext.Tickets.AddAsync(ticket);
            await _dataContext.SaveChangesAsync();

            return ticket;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND ticket.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            Ticket ticket = await _dataContext.Tickets
            .FromSqlRaw(querySql, param)
            .Include(ticket => ticket.passenger)
            .ThenInclude(passenger => passenger.flight_segment)
            .ThenInclude(flight_segment => flight_segment.aircraft)
            .ThenInclude(aircraft => aircraft.model)
            .FirstOrDefaultAsync();

            return ticket;
        }

        public async Task<PaginationResult<Ticket>> Pagination(PaginationFilter filter)
        {
            string querySql = $@"SELECT * ? tickets AS ticket WHERE ticket.excluded = false";
        
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);

            QueryAsyncParams queryAsyncParams = new QueryAsyncParams();

            queryAsyncParams.querySql = querySql;
            queryAsyncParams.tableName = "tickets";
            queryAsyncParams.relationshipName = "ticket";
            queryAsyncParams.queryParams = queryParams;
            queryAsyncParams.foreignKey = "ticket_id";

            Include includePassenger = new Include();
            string[] passengerAttributes = { "id", "name", "email", "document", "document_type" };
            includePassenger.attributes = passengerAttributes;
            includePassenger.foreignKey = "passenger_id";
            includePassenger.relationshipName = "passenger";
            includePassenger.tableName = "passengers";

            Include includeFlightSegment = new Include();
            string[] flightSegmentAttributes = { "id", "number", "origin_aerodrome_id", "destination_aerodrome_id", "aircraft_id", "flight_id" };
            includeFlightSegment.attributes = flightSegmentAttributes;
            includeFlightSegment.foreignKey = "flight_segment_id";
            includeFlightSegment.relationshipName = "flight_segment";
            includeFlightSegment.tableName = "flight_segments";

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

            includeFlightSegment.includes.Add(includeOriginAerodrome);
            includeFlightSegment.includes.Add(includeDestinationAerodrome);
            includeFlightSegment.includes.Add(includeAircraft);

            queryAsyncParams.includes.Add(includePassenger);
            queryAsyncParams.includes.Add(includeFlightSegment);

            IEnumerable<Ticket> tickets = await _orm.QueryAsync<Ticket>(queryAsyncParams);
            
            int total_records = await _dataContext.Tickets.CountAsync();
            PaginationResult<Ticket> paginationResult = _utils.CreatePaginationResult<Ticket>(tickets.ToList(), filter, total_records);

            return paginationResult;
        }

        public Task Update(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}