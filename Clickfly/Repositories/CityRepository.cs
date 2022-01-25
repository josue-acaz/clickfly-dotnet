using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using Npgsql;
using System.Collections.Generic;
using Dapper;
using clickfly.ViewModels;
using System.Linq;

namespace clickfly.Repositories
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "cities as city";
        private static string whereSql = "city.excluded = false";
        private static string innerJoinStates = "states as state on state.id = city.state_id";
        private static string deleteSql = "UPDATE cities SET excluded = true WHERE id = @id";

        public CityRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<City> Create(City city)
        {
            city.id = Guid.NewGuid().ToString();
            city.created_at = DateTime.Now;
            city.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = city;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<City>(options);

            return city;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public Task<City> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<City> GetByName(string name, string state_prefix)
        {
            string querySql = $@"SELECT 
                city.id, 
                city.name, 
                city.prefix, 
                city.description, 
                city.state_id, 
                city.timezone_id, 
                city.created_at, 
                city.updated_at
                FROM {fromSql} INNER JOIN {innerJoinStates} WHERE {whereSql} AND city.name ILIKE @name AND state.prefix = @state_prefix LIMIT 1";
            
            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("@name", $"%{name}%");
            queryParams.Add("@state_prefix", state_prefix);
            
            City city = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<City>(querySql, queryParams, _dBContext.GetTransaction());
            return city;
        }

        public async Task<PaginationResult<City>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string text = filter.text;

            string where = $"{whereSql} AND city.name ILIKE @text LIMIT @limit OFFSET @offset";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("text", $"%{text}%");

            SelectOptions options = new SelectOptions();
            options.As = "city";
            options.Where = where;
            options.Params = queryParams;

            options.Include<State>(new IncludeModel{
                As = "state",
                ForeignKey = "state_id"
            });

            options.Include<Timezone>(new IncludeModel{
                As = "timezone",
                ForeignKey = "timezone_id"
            });

            IEnumerable<City> cities = await _dapperWrapper.QueryAsync<City>(options);
            int total_records = cities.Count();

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<City> paginationResult = _utils.CreatePaginationResult<City>(cities.ToList(), paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<City> Update(City city)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = city;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<City>(options);
            return city;
        }
    }
}