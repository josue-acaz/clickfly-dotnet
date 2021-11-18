using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace clickfly.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "tickets as ticket";
        private static string whereSql = "ticket.excluded = false";
        private static string aircraftThumbnailSql = "SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";

        public TicketRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {

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
            IncludeModel includeState = new IncludeModel();
            includeState.As = "state";
            includeState.ForeignKey = "state_id";

            IncludeModel includeCity = new IncludeModel();
            includeCity.As = "city";
            includeCity.ForeignKey = "city_id";
            includeCity.ThenInclude<State>(includeState);

            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.AddRawAttribute("thumbnail", aircraftThumbnailSql);
            includeAircraft.ThenInclude<AircraftModel>(includeAircraftModel);

            IncludeModel includeOriginAerodrome = new IncludeModel();
            includeOriginAerodrome.As = "origin_aerodrome";
            includeOriginAerodrome.ForeignKey = "origin_aerodrome_id";
            includeOriginAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeDestinationAerodrome = new IncludeModel();
            includeDestinationAerodrome.As = "destination_aerodrome";
            includeDestinationAerodrome.ForeignKey = "destination_aerodrome_id";
            includeDestinationAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeFlightSegment = new IncludeModel();
            includeFlightSegment.As = "flight_segment";
            includeFlightSegment.ForeignKey = "flight_segment_id";
            includeFlightSegment.ThenInclude<Aircraft>(includeAircraft);
            includeFlightSegment.ThenInclude<Aerodrome>(includeOriginAerodrome);
            includeFlightSegment.ThenInclude<Aerodrome>(includeDestinationAerodrome);

            IncludeModel includePassenger = new IncludeModel();
            includePassenger.As = "passenger";
            includePassenger.ForeignKey = "passenger_id";

            QueryOptions options = new QueryOptions();

            options.As = "ticket";
            options.Where = $"{whereSql} AND ticket.id = @id";
            options.Params = new { id = id };
            options.Include<Passenger>(includePassenger);
            options.Include<FlightSegment>(includeFlightSegment);

            Ticket ticket = await _dBAccess.QuerySingleAsync<Ticket>(options);

            return ticket;
        }

        public async Task<PaginationResult<Ticket>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;

            IncludeModel includeState = new IncludeModel();
            includeState.As = "state";
            includeState.ForeignKey = "state_id";

            IncludeModel includeCity = new IncludeModel();
            includeCity.As = "city";
            includeCity.ForeignKey = "city_id";
            includeCity.ThenInclude<State>(includeState);

            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.AddRawAttribute("thumbnail", aircraftThumbnailSql);
            includeAircraft.ThenInclude<AircraftModel>(includeAircraftModel);

            IncludeModel includeOriginAerodrome = new IncludeModel();
            includeOriginAerodrome.As = "origin_aerodrome";
            includeOriginAerodrome.ForeignKey = "origin_aerodrome_id";
            includeOriginAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeDestinationAerodrome = new IncludeModel();
            includeDestinationAerodrome.As = "destination_aerodrome";
            includeDestinationAerodrome.ForeignKey = "destination_aerodrome_id";
            includeDestinationAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeFlightSegment = new IncludeModel();
            includeFlightSegment.As = "flight_segment";
            includeFlightSegment.ForeignKey = "flight_segment_id";
            includeFlightSegment.ThenInclude<Aircraft>(includeAircraft);
            includeFlightSegment.ThenInclude<Aerodrome>(includeOriginAerodrome);
            includeFlightSegment.ThenInclude<Aerodrome>(includeDestinationAerodrome);

            IncludeModel includePassenger = new IncludeModel();
            includePassenger.As = "passenger";
            includePassenger.ForeignKey = "passenger_id";

            QueryOptions options = new QueryOptions();
            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);

            options.Where = whereSql;
            options.Params = queryParams;
            options.Include<Passenger>(includePassenger);
            options.Include<FlightSegment>(includeFlightSegment);

            int total_records = await _dataContext.Tickets.CountAsync();
            IEnumerable<Ticket> tickets = await _dBAccess.QueryAsync<Ticket>(options);

            PaginationResult<Ticket> paginationResult = _utils.CreatePaginationResult<Ticket>(tickets.ToList(), filter, total_records);

            return paginationResult;
        }

        public Task Update(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}