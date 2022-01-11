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
        protected string[] defaultFields = new string[9];

        public PassengerRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            defaultFields[0] = "name";
            defaultFields[1] = "email";
            defaultFields[2] = "phone_number";
            defaultFields[3] = "emergency_phone_number";
            defaultFields[4] = "document";
            defaultFields[5] = "document_type";
            defaultFields[6] = "type";
            defaultFields[7] = "birthdate";
            defaultFields[8] = "customer_id";
        }

        public async Task<Passenger> Create(Passenger passenger)
        {
            passenger.id = Guid.NewGuid().ToString();

            await _dataContext.Passengers.AddAsync(passenger);
            await _dataContext.SaveChangesAsync();

            return passenger;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
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

        public Task<Passenger> Update(Passenger passenger)
        {
            throw new NotImplementedException();
        }
    }
}