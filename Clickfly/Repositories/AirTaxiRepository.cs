using System;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace clickfly.Repositories
{
    public class AirTaxiRepository : BaseRepository<AirTaxi>, IAirTaxiRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "air_taxis as air_taxi";
        private static string whereSql = "air_taxi.excluded = false";
        protected string[] defaultFields = new string[8];

        public AirTaxiRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {
            defaultFields[0] = "name";
            defaultFields[1] = "email";
            defaultFields[2] = "phone_number";
            defaultFields[3] = "cnpj";
        }
        
        public async Task<AirTaxi> Create(AirTaxi airTaxi)
        {
            string id = Guid.NewGuid().ToString();
            airTaxi.id = id;

            await _dataContext.AirTaxis.AddAsync(airTaxi);
            await _dataContext.SaveChangesAsync();

            return airTaxi;
        }

        public Task Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AirTaxi> GetById(string id, string[] fields = null)
        {
            if(fields != null)
            {
                GetFieldsSqlParams fieldsSqlParams = new GetFieldsSqlParams();
                fieldsSqlParams._as = "air_taxi";
                fieldsSqlParams.action = "GET";
                fieldsSqlParams.fields = fields;

                fieldsSql = _utils.GetFieldsSql(fieldsSqlParams);
            }

            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND air_taxi.id = @id";
            object param = new { id = id };
            
            AirTaxi airTaxi = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<AirTaxi>(querySql, param, _dBContext.GetTransaction());
            return airTaxi;
        }

        public Task<PaginationResult<AirTaxi>> Pagination(PaginationFilter filter)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(AirTaxi airTaxi)
        {
            throw new System.NotImplementedException();
        }
    }
}