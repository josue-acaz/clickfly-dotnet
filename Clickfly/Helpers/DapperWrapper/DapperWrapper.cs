using System;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace clickfly.Data
{
    public class DapperWrapper : QueryExtensions, IDapperWrapper
    {
        protected readonly IDBContext _dBContext;
        
        public DapperWrapper(IDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(SelectOptions options)
        {
            string pk = GetPK<T>();
            string querySql = GetSelectQuery<T>(options);
            Console.WriteLine(querySql);
            IEnumerable<dynamic> queryResult = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, options.Params, _dBContext.GetTransaction());
            
            Slapper.AutoMapper.Configuration.AddIdentifier(typeof(T), pk);
            IEnumerable<T> queryResultInstance = Slapper.AutoMapper.MapDynamic<T>(queryResult);

            return queryResultInstance;
        }

        public async Task<T> QuerySingleAsync<T>(SelectOptions options)
        {
            string pk = GetPK<T>();
            string querySql = GetSelectQuery<T>(options);
            
            IEnumerable<dynamic> queryResult = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, options.Params, _dBContext.GetTransaction());
            
            Slapper.AutoMapper.Configuration.AddIdentifier(typeof(T), pk);
            IEnumerable<T> queryResultInstance = Slapper.AutoMapper.MapDynamic<T>(queryResult);
            
            return queryResultInstance.FirstOrDefault<T>();
        }

        public async Task InsertAsync<T>(InsertOptions options)
        {
            InsertQueryParams queryParams = new InsertQueryParams();
            queryParams.Exclude = options.Exclude;

            string querySql = GetInsertQuery<T>(queryParams);
            await _dBContext.GetConnection().ExecuteAsync(querySql, options.Data, options.Transaction);
        }

        public async Task UpdateAsync<T>(UpdateOptions options)
        {
            UpdateQueryParams queryParams = new UpdateQueryParams();
            queryParams.Where = options.Where;
            queryParams.Exclude = options.Exclude;

            string querySql = GetUpdateQuery<T>(queryParams);
            await _dBContext.GetConnection().ExecuteAsync(querySql, options.Data, options.Transaction);
        }

        public int Count<T>(CountOptions options)
        {
            string As = options.As;
            string whereSql = options.Where;

            string tableName = GetTableName<T>();
            string querySql = $"SELECT COUNT(*) AS count FROM {tableName} AS {As}";
            
            if(whereSql != null)
            {
                querySql += $" WHERE {whereSql}";
            }

            int count = _dBContext.GetConnection().ExecuteScalar<int>(querySql, options.Params, options.Transaction);
            return count;
        }
    }
}
