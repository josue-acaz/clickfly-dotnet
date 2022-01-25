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
            string customerId = filter.customer_id;
            List<Booking> bookings = await _dataContext.Bookings
                .Include(booking => booking.status)
                .Include(booking => booking.payments)
                .Include(booking => booking.passengers)
                .Include(booking => booking.flight_segment)
                .ThenInclude(booking => booking.origin_aerodrome)
                .ThenInclude(origin_aerodrome => origin_aerodrome.city)
                .ThenInclude(city => city.state)
                .Include(booking => booking.flight_segment)
                .ThenInclude(booking => booking.destination_aerodrome)
                .ThenInclude(destination_aerodrome => destination_aerodrome.city)
                .ThenInclude(city => city.state)
                .Skip((filter.page_number - 1) * filter.page_size)
                .Take(filter.page_size)
                .Where(booking => booking.customer_id == customerId && booking.excluded == false)
                .ToListAsync();

            int total_records = await _dataContext.Bookings.CountAsync();
            PaginationResult<Booking> paginationResult = _utils.CreatePaginationResult<Booking>(bookings, filter, total_records);

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