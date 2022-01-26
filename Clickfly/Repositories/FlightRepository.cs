using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using Npgsql;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class FlightRepository : BaseRepository<Flight>, IFlightRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "flights as flight";
        private static string whereSql = "flight.excluded = false";

        public FlightRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Flight> Create(Flight flight)
        {
            flight.id = Guid.NewGuid().ToString();
            flight.created_at = DateTime.Now;
            flight.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = flight;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Flight>(options);
            return flight;
        }

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE flights set excluded = true WHERE id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<Flight> GetById(string id)
        {
            SelectOptions options = new SelectOptions();
            options.Where = $"{whereSql} AND flight.id = @id";
            options.Params = new { id = id };

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.ThenInclude<AircraftModel>(new IncludeModel{
                As = "model",
                ForeignKey = "aircraft_model_id",
            });

            IncludeModel includeSegments = new IncludeModel();
            includeSegments.As = "segments";
            includeSegments.ForeignKey = "flight_id";
            includeSegments.AddRawAttribute("status", "SELECT type FROM flight_segment_status WHERE flight_segment_id = segments.id ORDER BY created_at DESC LIMIT 1");

            includeSegments.ThenInclude<Aerodrome>(new IncludeModel{
                As = "origin_aerodrome",
                ForeignKey = "origin_aerodrome_id"
            });
            includeSegments.ThenInclude<Aerodrome>(new IncludeModel{
                As = "destination_aerodrome",
                ForeignKey = "destination_aerodrome_id"
            });

            options.Include<Aircraft>(includeAircraft);
            options.Include<FlightSegment>(includeSegments);

            Flight flight = await _dapperWrapper.QuerySingleAsync<Flight>(options);
            return flight;
        }

        public async Task<FlightSegment> GetLastSegment(string flight_id)
        {
            SelectOptions options = new SelectOptions();
            options.As = "flight_segment";
            options.Where = $"flight_segment.excluded = false AND flight_segment.flight_id = @flight_id ORDER BY flight_segment.number DESC LIMIT 1 OFFSET 0";
            options.Params = new { flight_id = flight_id };

            FlightSegment flightSegment = await _dapperWrapper.QuerySingleAsync<FlightSegment>(options);
            return flightSegment;
        }

        public async Task<PaginationResult<Flight>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string air_taxi_id = filter.air_taxi_id;
            string text = filter.text;

            // Ver o que fazer com a busca ILIKE
            string where = $"{whereSql} AND flight.air_taxi_id = @air_taxi_id LIMIT @limit OFFSET @offset";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("text", $"%{text}%");
            queryParams.Add("air_taxi_id", air_taxi_id);

            SelectOptions options = new SelectOptions();
            options.As = "flight";
            options.Where = where;
            options.Params = queryParams;

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.ThenInclude<AircraftModel>(new IncludeModel{
                As = "model",
                ForeignKey = "aircraft_model_id"
            });

            IncludeModel includeCity = new IncludeModel();
            includeCity.As = "city";
            includeCity.ForeignKey = "city_id";
            includeCity.ThenInclude<State>(new IncludeModel{
                As = "state",
                ForeignKey = "state_id"
            });

            IncludeModel includeOriginAerodrome = new IncludeModel();
            includeOriginAerodrome.As = "origin_aerodrome";
            includeOriginAerodrome.ForeignKey = "origin_aerodrome_id";
            includeOriginAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeDestinationAerodrome = new IncludeModel();
            includeDestinationAerodrome.As = "destination_aerodrome";
            includeDestinationAerodrome.ForeignKey = "destination_aerodrome_id";
            includeDestinationAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeSegments = new IncludeModel();
            includeSegments.As = "segments";
            includeSegments.ForeignKey = "flight_id";
            includeSegments.ThenInclude<Aerodrome>(includeOriginAerodrome);
            includeSegments.ThenInclude<Aerodrome>(includeDestinationAerodrome);
            includeSegments.AddRawAttribute("status", "SELECT type FROM flight_segment_status WHERE flight_segment_id = segments.id ORDER BY created_at DESC LIMIT 1");

            options.Include<Aircraft>(includeAircraft);
            options.Include<FlightSegment>(includeSegments);
            
            IEnumerable<Flight> flights = await _dapperWrapper.QueryAsync<Flight>(options);
            int total_records = flights.Count();

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<Flight> paginationResult = _utils.CreatePaginationResult<Flight>(flights.ToList(), paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<Flight> Update(Flight flight)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = flight;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<Flight>(options);
            return flight;
        }
    }
}