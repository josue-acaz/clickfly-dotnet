using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Npgsql;

namespace clickfly.Repositories
{
    public class StateRepository : BaseRepository<State>, IStateRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "states as state";
        private static string whereSql = "state.excluded = false";

        public StateRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {

        }

        public async Task<State> Create(State state)
        {
            string id = Guid.NewGuid().ToString();
            state.id = id;

            await _dataContext.States.AddAsync(state);
            await _dataContext.SaveChangesAsync();

            return state;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
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
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND state.prefix = @prefix";
            NpgsqlParameter param = new NpgsqlParameter("prefix", prefix);
            
            State state = await _dataContext.States.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return state;
        }

        public Task<PaginationResult<State>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(State state)
        {
            throw new NotImplementedException();
        }
    }
}