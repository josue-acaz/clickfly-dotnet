using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace clickfly.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils) : base(dBContext, dataContext, utils)
        {
            
        }

        public async Task<Booking> Create(Booking booking)
        {
            string id = Guid.NewGuid().ToString();
            booking.id = id;

            await _dataContext.Bookings.AddAsync(booking);
            await _dataContext.SaveChangesAsync();

            return booking;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<Booking>> Pagination(PaginationFilter filter)
        {
            string customerId = filter.customer_id;

            filter.page_number = 2;

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

        public Task Update(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}