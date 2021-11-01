using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Npgsql;
using System.Collections.Generic;
using Dapper;

namespace clickfly.Repositories
{
    public class CityRepository : BaseRepository<Timezone>, ICityRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "cities as city";
        private static string whereSql = "city.excluded = false";
        private static string innerJoinStates = "states as state on state.id = city.state_id";

        public CityRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils) : base(dBContext, dataContext, utils)
        {

        }

        public async Task<City> Create(City city)
        {
            string id = Guid.NewGuid().ToString();
            city.id = id;

            await _dataContext.Cities.AddAsync(city);
            await _dataContext.SaveChangesAsync();

            return city;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
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

        public Task<PaginationResult<City>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(City city)
        {
            throw new NotImplementedException();
        }
    }
}