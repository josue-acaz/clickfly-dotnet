using System;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class BookingStatusRepository : BaseRepository<BookingStatus>, IBookingStatusRepository
    {
        public BookingStatusRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {
            
        }

        public async Task<BookingStatus> Create(BookingStatus bookingStatus)
        {
            string id = Guid.NewGuid().ToString();
            bookingStatus.id = id;

            await _dataContext.BookingStatus.AddAsync(bookingStatus);
            await _dataContext.SaveChangesAsync();

            return bookingStatus;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BookingStatus> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<BookingStatus>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(BookingStatus bookingStatus)
        {
            throw new NotImplementedException();
        }
    }
}