using System;
using System.Collections.Generic;
using System.Data;
using clickfly.ViewModels;
using clickfly.Data;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace clickfly
{
    public class Orm : IOrm
    {
        private readonly IDBContext _dBContext;
        private readonly IDataContext _dataContext;
        
        public Orm(IDBContext dBContext, IDataContext dataContext)
        {
            _dBContext = dBContext;
            _dataContext = dataContext;
        }

        protected string GetJsonResult(dynamic result)
        {
            string jsonResult = JsonConvert.SerializeObject(result);
            jsonResult = jsonResult.Replace(@"\", string.Empty);
            jsonResult = jsonResult.Replace("\"{", "{");
            jsonResult = jsonResult.Replace("}\"", "}");

            return jsonResult;
        }

        protected string GetParentBuildModel(QueryAsyncParams queryAsyncParams)
        {
            string querySql = queryAsyncParams.querySql;
            string tableName = queryAsyncParams.tableName;
            string relationshipName = queryAsyncParams.relationshipName;
            string foreignKey = queryAsyncParams.foreignKey;

            var includes = queryAsyncParams.includes.ToArray();
            string[] queryIndex = querySql.Split(new[] { '?' }, 2);

            string leftSql = queryIndex[0];
            string rightSql = queryIndex[1];

            leftSql += ", ";
            for (int i = 0; i < includes.Length; i++)
            {
                var include = includes[i];
                string[] attributes = include.attributes;
                RawAttribute[] rawAttributes = include.rawAttributes;

                string buildModel = $"SELECT json_build_object(";
                for (int j = 0; j < attributes.Length; j++)
                {
                    string attribute = attributes[j];
                    var thenIncludes = include.includes.ToArray();
                    bool isLastAttribute = j == attributes.Length - 1;
                    buildModel += $"'{attribute}', {include.relationshipName}.{attribute}";
                
                    if(j < attributes.Length - 1 || thenIncludes.Length > 0)
                    {
                        buildModel += ",";
                    }

                    if(isLastAttribute)
                    {
                        buildModel = GetChildBuildModel(buildModel, include, thenIncludes);
                    }
                }

                for (int k = 0; k < rawAttributes.Length; k++)
                {
                    buildModel += ",";
                    RawAttribute rawAttribute = rawAttributes[k];

                    buildModel += $"'{rawAttribute.name}', ({rawAttribute.query})";
                }

                buildModel += $") FROM {include.tableName} AS {include.relationshipName} WHERE {relationshipName}.{include.foreignKey} = {include.relationshipName}.id";
                leftSql += $"({buildModel}) AS {include.relationshipName}";
                
                if(i < includes.Length - 1)
                {
                    leftSql += ",";
                }
            }

            string _querySql = $"SELECT * FROM ({leftSql} FROM {rightSql}) AS {relationshipName}";
            return _querySql;
        }

        protected string GetChildBuildModel(string buildModel, Include parent, Include[] includes)
        {
            for (int index = 0; index < includes.Length; index++)
            {
                var include = includes[index];
                var thenIncludes = include.includes.ToArray();
                string[] attributes = include.attributes;
                RawAttribute[] rawAttributes = include.rawAttributes;

                buildModel += $@"'{include.relationshipName}', (SELECT json_build_object(";

                for (int l = 0; l < attributes.Length; l++)
                {
                    string thenAttribute = attributes[l];
                    bool isLastAttribute = l == attributes.Length - 1;
                    buildModel += $"'{thenAttribute}', {include.relationshipName}.{thenAttribute}";
                
                    if(l < attributes.Length - 1 || thenIncludes.Length > 0)
                    {
                        buildModel += ",";
                    }

                    if(isLastAttribute)
                    {
                        buildModel = GetChildBuildModel(buildModel, include, thenIncludes);
                    }
                }

                for (int k = 0; k < rawAttributes.Length; k++)
                {
                    buildModel += ",";
                    RawAttribute rawAttribute = rawAttributes[k];

                    buildModel += $"'{rawAttribute.name}', ({rawAttribute.query})";
                }

                buildModel += $") FROM {include.tableName} AS {include.relationshipName} WHERE {parent.relationshipName}.{include.foreignKey} = {include.relationshipName}.id)";
            }

            return buildModel;
        }

        public async Task<IEnumerable<Type>> QueryAsync<Type>(QueryAsyncParams queryAsyncParams) where Type : new()
        {
            string querySql = GetParentBuildModel(queryAsyncParams);
            Dictionary<string, object> queryParams = queryAsyncParams.queryParams;

            IEnumerable<dynamic> result = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, queryParams);

            string jsonResult = GetJsonResult(result.ToArray());
            List<Type> data = JsonConvert.DeserializeObject<List<Type>>(jsonResult);
            return data.AsEnumerable();
        }

        public async Task<Type> QuerySingleOrDefaultAsync<Type>(QueryAsyncParams queryAsyncParams)
        {
            string querySql = GetParentBuildModel(queryAsyncParams);
            Dictionary<string, object> queryParams = queryAsyncParams.queryParams;

            dynamic result = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<dynamic>(querySql, queryParams);

            string jsonResult = GetJsonResult(result);
            Type data = JsonConvert.DeserializeObject<Type>(jsonResult);
            return data;
        }
    }
}