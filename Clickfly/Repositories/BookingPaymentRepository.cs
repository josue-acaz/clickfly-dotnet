using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;

namespace clickfly.Repositories
{
    public class BookingPaymentRepository : BaseRepository<BookingPayment>, IBookingPaymentRepository
    {
        private static string deleteSql = "UPDATE booking_payments SET excluded = true WHERE id = @id";
        
        public BookingPaymentRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            
        }

        public async Task<BookingPayment> Create(BookingPayment bookingPayment)
        {
            bookingPayment.created_at = DateTime.Now;
            bookingPayment.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = bookingPayment;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<BookingPayment>(options);
            return bookingPayment;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public Task<BookingPayment> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<BookingPayment>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingPayment> Update(BookingPayment bookingPayment)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = bookingPayment;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<BookingPayment>(options);
            return bookingPayment;
        }
    }
}