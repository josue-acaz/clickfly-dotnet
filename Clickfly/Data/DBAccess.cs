using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;
using Dapper;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Serialization;

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

        internal string GetQuery<Type>(QueryOptions queryOptions)
        {
            string AS = queryOptions.As;
            string where = queryOptions.Where;
            object optionParams = queryOptions.Params;
            List<IncludeModel> optionIncludes = queryOptions.Includes;

            string tableName = GetTableName<Type>();
            string[] defaultAttributes = queryOptions.Attributes.Length > 0 ? queryOptions.Attributes : GetAttributes<Type>();
            List<RawAttribute> rawAttributes = queryOptions.RawAttributes;

            CreateQueryParams createQueryParams = new CreateQueryParams();
            createQueryParams.TableName = tableName;
            createQueryParams.Attributes = defaultAttributes;
            createQueryParams.RawAttributes = rawAttributes;
            createQueryParams.Includes = optionIncludes;
            createQueryParams.Where = where;
            createQueryParams.As = AS;

            string querySql = CreateQuery(createQueryParams);
            return querySql;
        }

        internal Type SerializeResult<Type>(dynamic result, string As)
        {
            string queryResultJSON = JsonConvert.SerializeObject(result);
            queryResultJSON = queryResultJSON.Replace($"DapperRow, ", "");
            queryResultJSON = queryResultJSON.Replace($"{As}.", "");

            Console.WriteLine(queryResultJSON);

            DefaultContractResolver resolver = new DefaultContractResolver();
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = resolver, 
                Converters = new JsonConverter[] { 
                    new JsonFlatteningConverter(resolver)
                }
            };

            return(JsonConvert.DeserializeObject<Type>(queryResultJSON, settings));
        }

        public async Task<IEnumerable<Type>> QueryAsync<Type>(QueryOptions options)
        {
            string querySql = GetQuery<Type>(options);
            Console.WriteLine(querySql);
            dynamic queryResult = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, options.Params);
            return(SerializeResult<IEnumerable<Type>>(queryResult, options.As));
        }
    
        public async Task<Type> QuerySingleAsync<Type>(QueryOptions options)
        {
            QueryOptions queryOptions = options;
            queryOptions.Where += " LIMIT 1 ";
            string querySql = GetQuery<Type>(queryOptions);
            Console.WriteLine(querySql);
            dynamic queryResult = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<dynamic>(querySql, options.Params);
            return(SerializeResult<Type>(queryResult, options.As));
        }
    }
}
