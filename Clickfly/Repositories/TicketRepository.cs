using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using clickfly.ViewModels;
using Dapper;

namespace clickfly.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        protected readonly IUploadService _uploadService;
        private static string fieldsSql = "*";
        private static string fromSql = "tickets as ticket";
        private static string whereSql = "ticket.excluded = false";
        private static string deleteSql = "UPDATE tickets SET excluded = true WHERE id = @id";
        //private static string aircraftThumbnailSql = "SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";

        public TicketRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils, IUploadService uploadService) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            _uploadService = uploadService;
        }

        public async Task<Ticket> Create(Ticket ticket)
        {
            ticket.id = Guid.NewGuid().ToString();
            ticket.created_at = DateTime.Now;
            ticket.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = ticket;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Ticket>(options);
            return ticket;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public async Task<Ticket> GetById(string id)
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

            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.ThenInclude<AircraftModel>(includeAircraftModel);

            IncludeModel includeOriginAerodrome = new IncludeModel();
            includeOriginAerodrome.As = "origin_aerodrome";
            includeOriginAerodrome.ForeignKey = "origin_aerodrome_id";
            includeOriginAerodrome.ThenInclude<City>(includeOriginCity);

            IncludeModel includeDestinationAerodrome = new IncludeModel();
            includeDestinationAerodrome.As = "destination_aerodrome";
            includeDestinationAerodrome.ForeignKey = "destination_aerodrome_id";
            includeDestinationAerodrome.ThenInclude<City>(includeDestinationCity);

            IncludeModel includeAirTaxi = new IncludeModel();
            includeAirTaxi.As = "air_taxi";
            includeAirTaxi.ForeignKey = "air_taxi_id";

            IncludeModel includeFlight = new IncludeModel();
            includeFlight.As = "flight";
            includeFlight.ForeignKey = "flight_id";
            includeFlight.ThenInclude<AirTaxi>(includeAirTaxi);

            IncludeModel includeFlightSegment = new IncludeModel();
            includeFlightSegment.As = "flight_segment";
            includeFlightSegment.ForeignKey = "flight_segment_id";
            includeFlightSegment.ThenInclude<Aircraft>(includeAircraft);
            includeFlightSegment.ThenInclude<Aerodrome>(includeOriginAerodrome);
            includeFlightSegment.ThenInclude<Aerodrome>(includeDestinationAerodrome);
            includeFlightSegment.ThenInclude<Flight>(includeFlight);

            IncludeModel includePassenger = new IncludeModel();
            includePassenger.As = "passenger";
            includePassenger.ForeignKey = "passenger_id";

            SelectOptions options = new SelectOptions();

            options.As = "ticket";
            options.Where = $"{whereSql} AND ticket.id = @id AND ticket.booking_id = @booking_id";
            options.Params = new { id = id };
            options.Include<Passenger>(includePassenger);
            options.Include<FlightSegment>(includeFlightSegment);

            Ticket ticket = await _dapperWrapper.QuerySingleAsync<Ticket>(options);

            File thumbnailFile = await _dapperWrapper.QuerySingleAsync<File>(new SelectOptions{
                As = "file",
                Where = $"file.excluded = false AND file.resource_id = @aircraft_id AND file.field_name = 'thumbnail'",
                Params = new { aircraft_id = ticket.flight_segment.aircraft.id },
            });

            ticket.flight_segment.aircraft.thumbnail = _uploadService.GetPreSignedUrl(thumbnailFile.key);

            return ticket;
        }

        public async Task<PaginationResult<Ticket>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string booking_id = filter.booking_id;

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

            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            IncludeModel includeAircraft = new IncludeModel();
            includeAircraft.As = "aircraft";
            includeAircraft.ForeignKey = "aircraft_id";
            includeAircraft.ThenInclude<AircraftModel>(includeAircraftModel);

            IncludeModel includeOriginAerodrome = new IncludeModel();
            includeOriginAerodrome.As = "origin_aerodrome";
            includeOriginAerodrome.ForeignKey = "origin_aerodrome_id";
            includeOriginAerodrome.ThenInclude<City>(includeOriginCity);

            IncludeModel includeDestinationAerodrome = new IncludeModel();
            includeDestinationAerodrome.As = "destination_aerodrome";
            includeDestinationAerodrome.ForeignKey = "destination_aerodrome_id";
            includeDestinationAerodrome.ThenInclude<City>(includeDestinationCity);

            IncludeModel includeFlightSegment = new IncludeModel();
            includeFlightSegment.As = "flight_segment";
            includeFlightSegment.ForeignKey = "flight_segment_id";
            includeFlightSegment.ThenInclude<Aircraft>(includeAircraft);
            includeFlightSegment.ThenInclude<Aerodrome>(includeOriginAerodrome);
            includeFlightSegment.ThenInclude<Aerodrome>(includeDestinationAerodrome);

            IncludeModel includePassenger = new IncludeModel();
            includePassenger.As = "passenger";
            includePassenger.ForeignKey = "passenger_id";

            SelectOptions options = new SelectOptions();
            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("booking_id", booking_id);

            options.Where = whereSql;
            options.Params = queryParams;
            options.Include<Passenger>(includePassenger);
            options.Include<FlightSegment>(includeFlightSegment);

            int total_records = await _dataContext.Tickets.CountAsync();
            IEnumerable<Ticket> tickets = await _dapperWrapper.QueryAsync<Ticket>(options);

            List<Ticket> ticketsList = tickets.ToList();
            for (int i = 0; i < ticketsList.Count; i++)
            {
                File thumbnailFile = await _dapperWrapper.QuerySingleAsync<File>(new SelectOptions{
                    As = "file",
                    Where = $"file.excluded = false AND file.resource_id = @aircraft_id AND file.field_name = 'thumbnail'",
                    Params = new { aircraft_id = ticketsList[i].flight_segment.aircraft.id },
                });

                ticketsList[i].flight_segment.aircraft.thumbnail = _uploadService.GetPreSignedUrl(thumbnailFile.key);
            }

            PaginationResult<Ticket> paginationResult = _utils.CreatePaginationResult<Ticket>(tickets.ToList(), filter, total_records);

            return paginationResult;
        }

        public async Task<Ticket> Update(Ticket ticket)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = ticket;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<Ticket>(options);
            return ticket;
        }
    }
}