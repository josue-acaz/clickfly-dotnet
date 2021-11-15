using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace clickfly.Repositories
{
    public class AirTaxiBaseRepository : BaseRepository<AirTaxi>, IAirTaxiBaseRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "air_taxi_bases as air_taxi_base";
        private static string whereSql = "air_taxi_base.excluded = false";
        protected string[] defaultFields = new string[4];

        public AirTaxiBaseRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {
            defaultFields[0] = "name";
            defaultFields[1] = "latitude";
            defaultFields[2] = "longitude";
            defaultFields[3] = "aerodrome_id";
        }

        public async Task<AirTaxiBase> Create(AirTaxiBase airTaxiBase)
        {
            airTaxiBase.id = Guid.NewGuid().ToString();

            await _dataContext.AirTaxiBases.AddAsync(airTaxiBase);
            await _dataContext.SaveChangesAsync();

            return airTaxiBase;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AirTaxiBase> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND air_taxi_base.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            AirTaxiBase airTaxiBase = await _dataContext.AirTaxiBases
            .FromSqlRaw(querySql, param)
            .Include(airTaxiBase => airTaxiBase.aerodrome)
            .FirstOrDefaultAsync();

            return airTaxiBase;
        }

        public async Task<PaginationResult<AirTaxiBase>> Pagination(PaginationFilter filter)
        {
            string airTaxiId = filter.air_taxi_id;
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            List<AirTaxiBase> airTaxiBases = await _dataContext.AirTaxiBases
                .Include(airTaxiBase => airTaxiBase.aerodrome)
                .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
                .Take(paginationFilter.page_size)
                .Where(airTaxiBase => airTaxiBase.air_taxi_id == airTaxiId && airTaxiBase.excluded == false)
                .ToListAsync();
            
            int total_records = await _dataContext.AirTaxiBases.CountAsync();
            PaginationResult<AirTaxiBase> paginationResult = _utils.CreatePaginationResult<AirTaxiBase>(airTaxiBases, paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<AirTaxiBase> Update(AirTaxiBase airTaxiBase, string[] fields = null)
        {
            if(fields == null)
            {
                fields = defaultFields;
            }

            GetFieldsSqlParams fieldsSqlParams = new GetFieldsSqlParams();
            fieldsSqlParams.action = "UPDATE";
            fieldsSqlParams.fields = fields;

            string fieldsToUpdate = _utils.GetFieldsSql(fieldsSqlParams);
            string querySql = $"UPDATE air_taxi_bases SET {fieldsToUpdate} WHERE id = @id";

            await _dBContext.GetConnection().ExecuteAsync(querySql, airTaxiBase, _dBContext.GetTransaction());

            return airTaxiBase;
        }
    }
}
