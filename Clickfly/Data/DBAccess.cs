using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.ViewModels;
using Dapper;

/*
OBS.: Excluir foreignKey do mapeamento, o Slapper.Aumapper Crasha se por exemplo for mapeador ticker.passenger_id e passenger.id As passenger_id, já que teremos dois identificadores iguais passenger_id = passenger_id
*/
namespace clickfly.Data
{
    public class DBAccess : QueryExtensions, IDBAccess
    {
        private readonly IDBContext _dBContext;
        private readonly IDataContext _dataContext;
        
        public DBAccess(IDBContext dBContext, IDataContext dataContext)
        {
            _dBContext = dBContext;
            _dataContext = dataContext;
        }

        internal string GetQuery<T>(QueryOptions queryOptions)
        {
            string As = queryOptions.As;
            string where = queryOptions.Where;
            object optionParams = queryOptions.Params;
            List<IncludeModel> optionIncludes = queryOptions.Includes;

            if(As == null)
            {
                As = GetClassName<T>();
            }

            string tableName = GetTableName<T>();
            List<string> defaultAttributes = queryOptions.Attributes.Include.Count > 0 ? queryOptions.Attributes.Include : GetAttributes<T>(queryOptions.Attributes.Exclude);
            List<RawAttribute> rawAttributes = queryOptions.RawAttributes;

            CreateQueryParams createQueryParams = new CreateQueryParams();
            createQueryParams.TableName = tableName;
            createQueryParams.Attributes = defaultAttributes;
            createQueryParams.RawAttributes = rawAttributes;
            createQueryParams.Includes = optionIncludes;
            createQueryParams.Where = where;
            createQueryParams.As = As;

            string querySql = CreateQuery(createQueryParams);
            return querySql;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(QueryOptions options)
        {
            string querySql = GetQuery<T>(options);
            IEnumerable<dynamic> queryResult = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, options.Params);
            
            Slapper.AutoMapper.Configuration.AddIdentifier(typeof(T), "id");
            IEnumerable<T> queryResultInstance = Slapper.AutoMapper.MapDynamic<T>(queryResult);

            return queryResultInstance;
        }
    
        public async Task<T> QuerySingleAsync<T>(QueryOptions options)
        {
            string querySql = GetQuery<T>(options);
            IEnumerable<dynamic> queryResult = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, options.Params);
            
            Slapper.AutoMapper.Configuration.AddIdentifier(typeof(T), "id");
            IEnumerable<T> queryResultInstance = Slapper.AutoMapper.MapDynamic<T>(queryResult);
            
            return queryResultInstance.FirstOrDefault<T>();
        }
    }
}
