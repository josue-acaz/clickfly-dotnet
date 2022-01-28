using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Microsoft.EntityFrameworkCore;
using clickfly.ViewModels;
using Dapper;

namespace clickfly.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private static string whereSql = "booking.excluded = false";
        private static string deleteSql = "UPDATE bookings SET excluded = true WHERE id = @id";

        public BookingRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            
        }

        public async Task<Booking> Create(Booking booking)
        {
            booking.id = Guid.NewGuid().ToString();
            booking.created_at = DateTime.Now;
            booking.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = booking;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Booking>(options);
            return booking;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public Task<Booking> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<Booking>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string customer_id = filter.customer_id;
            string text = filter.text;

            string where = $"{whereSql} LIMIT @limit OFFSET @offset";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("customer_id", customer_id);
            queryParams.Add("text", $"%{text}%");

            SelectOptions options = new SelectOptions();
            options.As = "booking";
            options.Where = where;
            options.Params = queryParams;

            options.Include<BookingStatus>(new IncludeModel{
                As = "status",
                ForeignKey = "booking_id"
            });
            options.Include<BookingPayment>(new IncludeModel{
                As = "payments",
                ForeignKey = "booking_id"
            });
            options.Include<Passenger>(new IncludeModel{
                As = "passengers",
                ForeignKey = "booking_id"
            });

            IncludeModel includeCity = new IncludeModel();
            includeCity.As = "city";
            includeCity.ForeignKey = "city_id";
            //includeCity.AddRawAttribute("full_name", "CONCAT(city.name, ' • ', state.prefix)");
            includeCity.ThenInclude<State>(new IncludeModel{
                As = "state",
                ForeignKey = "state_id"
            });

            IncludeModel includeOriginAerodrome = new IncludeModel();
            includeOriginAerodrome.As = "origin_aerodrome";
            includeOriginAerodrome.ForeignKey = "origin_aerodrome_id";
            includeOriginAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeDestinAerodrome = new IncludeModel();
            includeDestinAerodrome.As = "destination_aerodrome";
            includeDestinAerodrome.ForeignKey = "destination_aerodrome_id";
            includeDestinAerodrome.ThenInclude<City>(includeCity);

            IncludeModel includeFlightSegment = new IncludeModel();
            includeFlightSegment.As = "flight_segment";
            includeFlightSegment.ForeignKey = "flight_segment_id";
            includeFlightSegment.ThenInclude<Aerodrome>(includeOriginAerodrome);
            includeFlightSegment.ThenInclude<Aerodrome>(includeDestinAerodrome);

            options.Include<FlightSegment>(includeFlightSegment);

            IEnumerable<Booking> bookings = await _dapperWrapper.QueryAsync<Booking>(options);
            int total_records = bookings.Count();

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<Booking> paginationResult = _utils.CreatePaginationResult<Booking>(bookings.ToList(), filter, total_records);

            return paginationResult;
        }

        public async Task<Booking> Update(Booking booking)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = booking;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<Booking>(options);
            return booking;
        }
    }
}