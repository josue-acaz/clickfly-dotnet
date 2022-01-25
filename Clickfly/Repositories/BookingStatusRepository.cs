using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;

namespace clickfly.Repositories
{
    public class BookingStatusRepository : BaseRepository<BookingStatus>, IBookingStatusRepository
    {
        private static string deleteSql = "UPDATE booking_status SET excluded = true WHERE id = @id";

        public BookingStatusRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            
        }

        public async Task<BookingStatus> Create(BookingStatus bookingStatus)
        {
            bookingStatus.id = Guid.NewGuid().ToString();
            bookingStatus.created_at = DateTime.Now;
            bookingStatus.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = bookingStatus;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<BookingStatus>(options);
            return bookingStatus;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public Task<BookingStatus> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<BookingStatus>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingStatus> Update(BookingStatus bookingStatus)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = bookingStatus;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<BookingStatus>(options);
            return bookingStatus;
        }
    }
}