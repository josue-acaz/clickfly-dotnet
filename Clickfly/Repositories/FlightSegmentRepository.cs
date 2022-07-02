using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class FlightSegmentRepository : BaseRepository<FlightSegment>, IFlightSegmentRepository
    {
        protected readonly IUploadService _uploadService;
        private static string fieldsSql = "*";
        private static string fromSql = "flight_segments as flight_segment";
        private static string whereSql = "flight_segment.excluded = false";
        private static string deleteSql = "UPDATE flight_segments SET excluded = true WHERE id = @id";
        private static string availableSeatsSql = $"flight_segment.total_seats - get_booked_seats(flight_segment.id)";
        private static string flightTimeSql = $"(SELECT EXTRACT(EPOCH FROM (flight_segment.arrival_datetime - flight_segment.departure_datetime))) / 60";
        //private static string aircraftThumbnailSql = $"SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";

        public FlightSegmentRepository(
                IDBContext dBContext, 
                IDataContext dataContext, 
                IDapperWrapper dapperWrapper, 
                IUploadService uploadService,
                IUtils utils
        ) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            _uploadService = uploadService;
        }

        public async Task<FlightSegment> Create(FlightSegment flightSegment)
        {
            flightSegment.id = Guid.NewGuid().ToString();
            flightSegment.created_at = DateTime.Now;
            flightSegment.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = flightSegment;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<FlightSegment>(options);
            return flightSegment;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public async Task<FlightSegment> GetById(string id)
        {
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

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.ThenInclude<AircraftModel>(new IncludeModel{
                As = "model",
                ForeignKey = "aircraft_model_id"
            });

            IncludeModel includeFlight = new IncludeModel();
            includeFlight.As = "flight";
            includeFlight.ForeignKey = "flight_id";

            SelectOptions options = new SelectOptions();
            options.As = "flight_segment";
            options.Where = $"{whereSql} AND flight_segment.id = @id";
            options.Params = new { id = id };
            options.AddRawAttribute("available_seats", availableSeatsSql);
            options.Include<Flight>(includeFlight);
            options.Include<Aircraft>(includeAircraft);
            options.Include<Aerodrome>(includeOriginAerodrome);
            options.Include<Aerodrome>(includeDestinationAerodrome);
            
            FlightSegment flightSegment = await _dapperWrapper.QuerySingleAsync<FlightSegment>(options);
            File thumbnailFile = await _dapperWrapper.QuerySingleAsync<File>(new SelectOptions{
                As = "file",
                Where = $"file.excluded = false AND file.resource_id = @aircraft_id AND file.field_name = 'thumbnail'",
                Params = new { aircraft_id = flightSegment.aircraft.id },
            });

            flightSegment.aircraft.thumbnail = _uploadService.GetPreSignedUrl(thumbnailFile.key);

            return flightSegment;
        }

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string flight_id = filter.flight_id;
            string text = filter.text;
            string where = $"{whereSql}";
            string mainWhere = $"flight.excluded = false";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("text", $"%{text}%");

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

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.ThenInclude<AircraftModel>(new IncludeModel{
                As = "model",
                ForeignKey = "aircraft_model_id"
            });

            SelectOptions options = new SelectOptions();
            options.As = "flight_segment";
            options.Params = queryParams;
            options.Include<Aircraft>(includeAircraft);
            options.Include<Aerodrome>(includeOriginAerodrome);
            options.Include<Aerodrome>(includeDestinationAerodrome);
            options.Include<Flight>(new IncludeModel{ As = "flight", ForeignKey = "flight_id" });

            if(flight_id != null)
            {
                where += " AND flight_segment.flight_id = @flight_id ";
                queryParams.Add("flight_id", flight_id);
            }
            else
            {
                mainWhere += " AND double_check.resource = 'flight_segments' AND double_check.approved IS NULL ";
                
                IncludeModel includeDoubleCheck = new IncludeModel();
                includeDoubleCheck.As = "double_check";
                includeDoubleCheck.ForeignKey = "resource_id";

                includeDoubleCheck.ThenInclude<User>(new IncludeModel{
                    As = "_user",
                    ForeignKey = "user_id"
                });

                options.AddRawAttribute("user_name", "_user.name");
                options.Include<DoubleCheck>(includeDoubleCheck);
            }

            where += " LIMIT @limit OFFSET @offset ";
            options.Where = where;
            options.MainWhere = mainWhere;

            IEnumerable<FlightSegment> flightSegments = await _dapperWrapper.QueryAsync<FlightSegment>(options);
            int total_records = flightSegments.Count();

            PaginationResult<FlightSegment> paginationResult = _utils.CreatePaginationResult<FlightSegment>(flightSegments.ToList(), filter, total_records);

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
