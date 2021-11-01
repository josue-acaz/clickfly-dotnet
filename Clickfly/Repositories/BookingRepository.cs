using System;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;

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

        public Task<PaginationResult<Booking>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}