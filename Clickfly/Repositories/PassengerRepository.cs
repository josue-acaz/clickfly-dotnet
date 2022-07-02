using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class PassengerRepository : BaseRepository<CustomerFriend>, IPassengerRepository 
    {
        private static string fieldsSql = "*";
        private static string fromSql = "passengers as passenger";
        private static string whereSql = "passenger.excluded = false";
        private static string deleteSql = "UPDATE passengers SET excluded = true WHERE id = @id";

        public PassengerRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Passenger> Create(Passenger passenger)
        {
            passenger.id = Guid.NewGuid().ToString();
            passenger.created_at = DateTime.Now;
            passenger.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = passenger;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Passenger>(options);
            return passenger;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public Task<Passenger> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<Passenger>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task RangeCreate(Passenger[] passengers)
        {
            await _dataContext.Passengers.AddRangeAsync(passengers);
        }

        public async Task<Passenger> Update(Passenger passenger)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = passenger;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<Passenger>(options);
            return passenger;
        }
    }
}