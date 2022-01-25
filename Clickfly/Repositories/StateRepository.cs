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
    public class StateRepository : BaseRepository<State>, IStateRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "states as state";
        private static string whereSql = "state.excluded = false";
        private static string deleteSql = "UPDATE states SET excluded = true WHERE id = @id";

        public StateRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<State> Create(State state)
        {
            state.id = Guid.NewGuid().ToString();
            state.created_at = DateTime.Now;
            state.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = state;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<State>(options);
            return state;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public async Task<State> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND state.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            State state = await _dataContext.States.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return state;
        }

        public async Task<State> GetByPrefix(string prefix)
        {
            SelectOptions options = new SelectOptions();
            options.As = "state";
            options.Where = $"{whereSql} AND state.prefix = @prefix LIMIT 1";
            options.Params = new { prefix = prefix };
            
            State state = await _dapperWrapper.QuerySingleAsync<State>(options);
            return state;
        }

        public async Task<PaginationResult<State>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string text = filter.text;

            string where = $"{whereSql} AND state.name ILIKE @text OR state.prefix ILIKE @text LIMIT @limit OFFSET @offset";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("text", $"%{text}%");

            SelectOptions options = new SelectOptions();
            options.As = "state";
            options.Where = where;
            options.Params = queryParams;

            IEnumerable<State> cities = await _dapperWrapper.QueryAsync<State>(options);
            int total_records = cities.Count();

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<State> paginationResult = _utils.CreatePaginationResult<State>(cities.ToList(), paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<State> Update(State state)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = state;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<State>(options);
            return state;
        }
    }
}