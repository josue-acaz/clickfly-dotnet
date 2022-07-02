using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class AppFlightRepository : BaseRepository<FlightSegment>, IAppFlightRepository
    {
        protected readonly IUploadService _uploadService;

        private static string whereSql = $"flight_segment.excluded = false";
        private static string availableSeatsSql = $"flight_segment.total_seats - get_booked_seats(flight_segment.id)";
        private static string flightTimeSql = $"(SELECT EXTRACT(EPOCH FROM (flight_segment.arrival_datetime - flight_segment.departure_datetime))) / 60";
        private static string subtotalSql = $"SELECT flight_segment.price_per_seat * @selected_seats";
        //private static string aircraftThumbnailSql = "SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";

        public AppFlightRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils, IUploadService uploadService) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            _uploadService = uploadService;
        }

        public async Task<PaginationResult<AppFlight>> Overview(PaginationFilter filter)
        {
            string now = _utils.DateTimeToSql(DateTime.Now);

            string availableFlightCitySql = $@"
                SELECT DISTINCT ON(origin_city_id, destination_city_id) origin_aerodrome.city_id AS origin_city_id, destination_aerodrome.city_id AS destination_city_id FROM flight_segments AS flight_segment 
                INNER JOIN flights AS flight ON flight_segment.flight_id = flight.id
                INNER JOIN aerodromes AS origin_aerodrome ON flight_segment.origin_aerodrome_id = origin_aerodrome.id
                INNER JOIN aerodromes AS destination_aerodrome ON flight_segment.destination_aerodrome_id = destination_aerodrome.id
                WHERE flight_segment.excluded = false 
                AND flight.type = 'shared' 
                AND flight_segment.type = 'trip' 
                AND (flight_segment.total_seats > get_booked_seats(flight_segment.id))
                AND flight_segment.departure_datetime > current_timestamp + (120 * interval '1 minute')
            ";

            string countSql = $@"
                SELECT COUNT(available_flight_cities) AS total_records FROM ({availableFlightCitySql}) AS available_flight_cities GROUP BY available_flight_cities.origin_city_id, available_flight_cities.destination_city_id
            ";

            string querySql = $@"
                SELECT (
                    SELECT json_build_object(
                        'id', city.id, 
                        'name', city.name, 
                        'state_id', city.state_id,
                        'state', (
                            SELECT json_build_object('id', state.id, 'prefix', state.prefix) FROM states AS state WHERE state.id = city.state_id
                        )
                    ) FROM cities AS city WHERE city.excluded = false AND city.id = available_flight_cities.origin_city_id
                ) AS origin_city,
                (
                    SELECT json_build_object(
                        'id', city.id, 
                        'name', city.name, 
                        'state_id', city.state_id,
                        'state', (
                            SELECT json_build_object('id', state.id, 'prefix', state.prefix) FROM states AS state WHERE state.id = city.state_id
                        )
                    ) FROM cities AS city WHERE city.excluded = false AND city.id = available_flight_cities.destination_city_id
                ) AS destination_city,
                (
                    SELECT json_agg(flight_segment.*) FROM (
                        SELECT flight_segment.id,
                        flight_segment.number,
                        flight_segment.departure_datetime,
                        flight_segment.arrival_datetime,
                        flight_segment.price_per_seat,
                        flight_segment.total_seats,
                        flight_segment.type, 
                        (
                            SELECT json_build_object(
                                'id', aerodrome.id,
                                'name', aerodrome.name,
                                'oaci_code', aerodrome.oaci_code,
                                'ciad', aerodrome.ciad
                            ) FROM aerodromes AS aerodrome 
                            WHERE aerodrome.excluded = false 
                            AND aerodrome.id = flight_segment.origin_aerodrome_id
                        ) AS origin_aerodrome,
                        (
                            SELECT json_build_object(
                                'id', aerodrome.id,
                                'name', aerodrome.name,
                                'oaci_code', aerodrome.oaci_code,
                                'ciad', aerodrome.ciad
                            ) FROM aerodromes AS aerodrome 
                            WHERE aerodrome.excluded = false 
                            AND aerodrome.id = flight_segment.destination_aerodrome_id
                        ) AS destination_aerodrome
                        FROM flight_segments AS flight_segment 
                        INNER JOIN aerodromes AS origin_aerodrome ON flight_segment.origin_aerodrome_id = origin_aerodrome.id
                        INNER JOIN aerodromes AS destination_aerodrome ON flight_segment.destination_aerodrome_id = destination_aerodrome.id
                        WHERE flight_segment.excluded = false 
                        AND origin_aerodrome.city_id = available_flight_cities.origin_city_id
                        AND destination_aerodrome.city_id = available_flight_cities.destination_city_id
                    ) AS flight_segment
                ) AS flights
                FROM ({availableFlightCitySql}) AS available_flight_cities GROUP BY available_flight_cities.origin_city_id, available_flight_cities.destination_city_id
            ";

            IEnumerable<dynamic> result = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, null, _dBContext.GetTransaction());
            List<AppFlight> appFlights = new List<AppFlight>();
    
            dynamic[] data = result.ToArray<dynamic>();
            for (int i = 0; i < data.Length; i++)
            {
                dynamic obj = data[i];
                City originCity = JsonSerializer.Deserialize<City>(obj.origin_city);
                City destinationCity = JsonSerializer.Deserialize<City>(obj.destination_city);
                List<FlightSegment> flights = new List<FlightSegment>(JsonSerializer.Deserialize<List<FlightSegment>>(obj.flights));
            
                AppFlight appFlight = new AppFlight();
                appFlight.origin_city = originCity;
                appFlight.destination_city = destinationCity;
                appFlight.flights = flights;

                appFlights.Add(appFlight);
            }

            int total_records = await _dBContext.GetConnection().QueryFirstOrDefaultAsync<int>(countSql);
            PaginationResult<AppFlight> paginationResult = _utils.CreatePaginationResult<AppFlight>(appFlights, filter, total_records);
            
            return paginationResult;
        } 

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            int selected_seats = filter.selected_seats;

            string flight_type = filter.flight_type;
            string origin_city_id = filter.origin_city_id;
            string destination_city_id = filter.destination_city_id;
            string currentDatetime = _utils.DateTimeToSql(DateTime.Now);

            IncludeModel includeOriginCity = new IncludeModel();
            includeOriginCity.As = "city";
            includeOriginCity.ForeignKey = "city_id";
            includeOriginCity.ThenInclude<State>(new IncludeModel{
                As = "state",
                ForeignKey = "state_id"
            });

            IncludeModel includeDestinationCity = new IncludeModel();
            includeDestinationCity.As = "city";
            includeDestinationCity.ForeignKey = "city_id";
            includeDestinationCity.ThenInclude<State>(new IncludeModel{
                As = "state",
                ForeignKey = "state_id"
            });

            IncludeModel includeOriginAerodrome = new IncludeModel();
            includeOriginAerodrome.As = "origin_aerodrome";
            includeOriginAerodrome.ForeignKey = "origin_aerodrome_id";
            includeOriginAerodrome.ThenInclude<City>(includeOriginCity);

            IncludeModel includeDestinationAerodrome = new IncludeModel();
            includeDestinationAerodrome.As = "destination_aerodrome";
            includeDestinationAerodrome.ForeignKey = "destination_aerodrome_id";
            includeDestinationAerodrome.ThenInclude<City>(includeDestinationCity);

            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.AddRawAttribute("flight_time", flightTimeSql);
            //includeAircraft.AddRawAttribute("thumbnail", aircraftThumbnailSql);
            includeAircraft.ThenInclude<AircraftModel>(includeAircraftModel);

            IncludeModel includeFlight = new IncludeModel();
            includeFlight.As = "flight";
            includeFlight.ForeignKey = "flight_id";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("flight_type", flight_type);
            queryParams.Add("origin_city_id", origin_city_id);
            queryParams.Add("destination_city_id", destination_city_id);
            queryParams.Add("selected_seats", selected_seats);

            SelectOptions options = new SelectOptions();
            options.As = "flight_segment";
            options.Params = queryParams;
            options.Where = $@"{whereSql}
                AND flight_segment.type = 'trip'
                AND (({availableSeatsSql}) - @selected_seats) >= 0 
                AND flight_segment.departure_datetime > '{currentDatetime}'::date + (120 * interval '1 minute')
                LIMIT @limit OFFSET @offset
            ";
            options.MainWhere = $@"
                flight.type = @flight_type
                AND origin_aerodrome.city_id = @origin_city_id
                AND destination_aerodrome.city_id = @destination_city_id
            ";

            options.AddRawAttribute("subtotal", subtotalSql);
            options.AddRawAttribute("available_seats", availableSeatsSql);

            options.Include<Flight>(includeFlight);
            options.Include<Aircraft>(includeAircraft);
            options.Include<Aerodrome>(includeOriginAerodrome);
            options.Include<Aerodrome>(includeDestinationAerodrome);

            IEnumerable<FlightSegment> flightSegments = await _dapperWrapper.QueryAsync<FlightSegment>(options);
            List<FlightSegment> flightSegmentsList = flightSegments.ToList();

            for (int i = 0; i < flightSegmentsList.Count; i++)
            {
                File thumbnailFile = await _dapperWrapper.QuerySingleAsync<File>(new SelectOptions{
                    As = "file",
                    Where = $"file.excluded = false AND file.resource_id = @aircraft_id AND file.field_name = 'thumbnail'",
                    Params = new { aircraft_id = flightSegmentsList[i].aircraft.id },
                });

                flightSegmentsList[i].aircraft.thumbnail = _uploadService.GetPreSignedUrl(thumbnailFile.key);
            }

            int total_records = flightSegments.Count();
            PaginationResult<FlightSegment> paginationResult = _utils.CreatePaginationResult<FlightSegment>(flightSegmentsList, filter, total_records);

            return paginationResult;
        }
    }
}