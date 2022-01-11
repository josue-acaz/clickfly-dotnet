using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using Npgsql;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class TimezoneRepository : BaseRepository<Timezone>, ITimezoneRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "timezones as timezone";
        private static string whereSql = "timezone.excluded = false";

        public TimezoneRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Timezone> Create(Timezone timezone)
        {
            string id = Guid.NewGuid().ToString();
            timezone.id = id;

            await _dataContext.Timezones.AddAsync(timezone);
            await _dataContext.SaveChangesAsync();

            return timezone;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Timezone> GetByGmt(int gmt)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND timezone.gmt = @gmt";
            NpgsqlParameter param = new NpgsqlParameter("gmt", gmt);
            
            Timezone timezone = await _dataContext.Timezones.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return timezone;
        }

        public Task<Timezone> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<Timezone>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(Timezone timezone)
        {
            throw new NotImplementedException();
        }
    }
}