using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using Npgsql;
using clickfly.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace clickfly.Repositories
{
    public class TimezoneRepository : BaseRepository<Timezone>, ITimezoneRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "timezones as timezone";
        private static string whereSql = "timezone.excluded = false";
        private static string deleteSql = "UPDATE timezones SET excluded = true WHERE id = @id";

        public TimezoneRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Timezone> Create(Timezone timezone)
        {
            timezone.id = Guid.NewGuid().ToString();
            timezone.created_at = DateTime.Now;
            timezone.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = timezone;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Timezone>(options);
            return timezone;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public async Task<Timezone> GetByGmt(int gmt)
        {
            SelectOptions options = new SelectOptions();
            options.As = "timezone";
            options.Where = $"{whereSql} AND timezone.gmt = @gmt LIMIT 1";
            options.Params = new { gmt = gmt };
            
            Timezone timezone = await _dapperWrapper.QuerySingleAsync<Timezone>(options);
            return timezone;
        }

        public Task<Timezone> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<Timezone>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string text = filter.text;

            string where = $"{whereSql} AND CAST(timezone.gmt AS TEXT) ILIKE @text LIMIT @limit OFFSET @offset";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("text", $"%{text}%");

            SelectOptions options = new SelectOptions();
            options.As = "timezone";
            options.Where = where;
            options.Params = queryParams;

            IEnumerable<Timezone> timezones = await _dapperWrapper.QueryAsync<Timezone>(options);
            int total_records = timezones.Count();

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<Timezone> paginationResult = _utils.CreatePaginationResult<Timezone>(timezones.ToList(), paginationFilter, total_records);

            return paginationResult;
        }

        public Task<Timezone> Update(Timezone timezone)
        {
            throw new NotImplementedException();
        }
    }
}