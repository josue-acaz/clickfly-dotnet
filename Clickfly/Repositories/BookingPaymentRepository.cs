using System;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class BookingPaymentRepository : BaseRepository<BookingPayment>, IBookingPaymentRepository
    {
        public BookingPaymentRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils) : base(dBContext, dataContext, utils)
        {
            
        }

        public async Task<BookingPayment> Create(BookingPayment bookingPayment)
        {
            string id = Guid.NewGuid().ToString();
            bookingPayment.id = id;

            await _dataContext.BookingPayments.AddAsync(bookingPayment);
            await _dataContext.SaveChangesAsync();

            return bookingPayment;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BookingPayment> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<BookingPayment>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(BookingPayment booking)
        {
            throw new NotImplementedException();
        }
    }
}