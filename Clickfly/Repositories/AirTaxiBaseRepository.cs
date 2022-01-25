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
    public class AirTaxiBaseRepository : BaseRepository<AirTaxi>, IAirTaxiBaseRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "air_taxi_bases as air_taxi_base";
        private static string whereSql = "air_taxi_base.excluded = false";
        private static string deleteSql = "UPDATE air_taxi_bases SET excluded = true WHERE id = @id";

        public AirTaxiBaseRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<AirTaxiBase> Create(AirTaxiBase airTaxiBase)
        {
            airTaxiBase.id = Guid.NewGuid().ToString();
            airTaxiBase.created_at = DateTime.Now;
            airTaxiBase.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = airTaxiBase;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<AirTaxiBase>(options);
            return airTaxiBase;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
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

        public async Task<AirTaxiBase> Update(AirTaxiBase airTaxiBase)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = airTaxiBase;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<AirTaxiBase>(options);
            return airTaxiBase;
        }
    }
}
