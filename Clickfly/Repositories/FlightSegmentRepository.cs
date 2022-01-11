using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class FlightSegmentRepository : BaseRepository<FlightSegment>, IFlightSegmentRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "flight_segments as flight_segment";
        private static string whereSql = "flight_segment.excluded = false";
        private static string availableSeatsSql = $"flight_segment.total_seats - get_booked_seats(flight_segment.id)";
        private static string flightTimeSql = $"(SELECT EXTRACT(EPOCH FROM (flight_segment.arrival_datetime - flight_segment.departure_datetime))) / 60";
        private static string aircraftThumbnailSql = $"SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";
        protected string[] defaultFields = new string[10];

        public FlightSegmentRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {
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
            IncludeModel includeState = new IncludeModel();
            includeState.As = "state";
            includeState.ForeignKey = "state_id";

            IncludeModel includeCity = new IncludeModel();
            includeCity.As = "city";
            includeCity.ForeignKey = "city_id";
            includeCity.ThenInclude<State>(includeState);

            IncludeModel includeOriginAerodrome = new IncludeModel();
            includeOriginAerodrome.As = "origin_aerodrome";
            includeOriginAerodrome.ForeignKey = "origin_aerodrome_id";
            includeOriginAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeDestinationAerodrome = new IncludeModel();
            includeDestinationAerodrome.As = "destination_aerodrome";
            includeDestinationAerodrome.ForeignKey = "destination_aerodrome_id";
            includeDestinationAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.ThenInclude<AircraftModel>(includeAircraftModel);
            includeAircraft.AddRawAttribute("thumbnail", aircraftThumbnailSql);

            IncludeModel includeFlight = new IncludeModel();
            includeFlight.As = "flight";
            includeFlight.ForeignKey = "flight_id";

            SelectOptions options = new SelectOptions();
            options.As = "flight_segment";
            options.Where = $"{whereSql} AND flight_segment.id = @id";
            options.Params = new { id = id };
            options.AddRawAttribute("flight_time", flightTimeSql);
            options.AddRawAttribute("available_seats", availableSeatsSql);
            
            options.Include<Flight>(includeFlight);
            options.Include<Aircraft>(includeAircraft);
            options.Include<Aerodrome>(includeOriginAerodrome);
            options.Include<Aerodrome>(includeDestinationAerodrome);
            
            FlightSegment flightSegment = await _dapperWrapper.QuerySingleAsync<FlightSegment>(options);
            
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

        public async Task<FlightSegment> Update(FlightSegment flightSegment)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = flightSegment;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<FlightSegment>(options);
            return flightSegment;
        }
    }
}
