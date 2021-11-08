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
            jsonResult = jsonResult.Replace(@"\n", string.Empty); // Remove quebra de linha inserida
            jsonResult = jsonResult.Replace(@"\", string.Empty);
            jsonResult = jsonResult.Replace("\"{", "{");
            jsonResult = jsonResult.Replace("}\"", "}");
            jsonResult = jsonResult.Replace("\"[", "[");
            jsonResult = jsonResult.Replace("]\"", "]");

            return jsonResult;
        }

        protected string GetParentBuildModel(QueryAsyncParams queryAsyncParams)
        {
            string querySql = queryAsyncParams.querySql;
            string tableName = queryAsyncParams.tableName;
            string relationshipName = queryAsyncParams.relationshipName;
            string foreignKey = queryAsyncParams.foreignKey;
            RawAttribute[] rawAttributes = queryAsyncParams.rawAttributes;

            var includes = queryAsyncParams.includes.ToArray();
            string[] queryIndex = querySql.Split(new[] { '?' }, 2);

            string leftSql = queryIndex[0];
            string rightSql = queryIndex[1];
            bool hasIncludes  = includes.Length > 0;

            if(hasIncludes)
            {
                leftSql += ", ";
            }

            for (int i = 0; i < includes.Length; i++)
            {
                var include = includes[i];
                string where = include.where;
                bool hasMany = include.hasMany;
                string[] attributes = include.attributes;
                RawAttribute[] includeRawAttributes = include.rawAttributes;
                string buildModel = $"SELECT ";

                if(hasMany)
                {
                    buildModel += $@"json_agg({include.relationshipName}.*) FROM (
                        SELECT * FROM {include.tableName} AS {include.relationshipName} WHERE {include.relationshipName}.{include.foreignKey} = {relationshipName}.id";

                    if(where != null)
                    {
                        buildModel += $" AND {where}";
                    }

                    buildModel += $") AS {include.relationshipName}";
                }
                else
                {
                    buildModel += $"json_build_object(";
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

                    for (int k = 0; k < includeRawAttributes.Length; k++)
                    {
                        buildModel += ",";
                        RawAttribute rawAttribute = includeRawAttributes[k];

                        buildModel += $"'{rawAttribute.name}', ({rawAttribute.query})";
                    }

                    buildModel += $") FROM {include.tableName} AS {include.relationshipName} WHERE {relationshipName}.{include.foreignKey} = {include.relationshipName}.id";
                }

                leftSql += $"({buildModel}) AS {include.relationshipName}";

                if(i < includes.Length - 1)
                {
                    leftSql += ",";
                }
            }

            // Atributos dinâmicos da consulta principal
            for (int k = 0; k < rawAttributes.Length; k++)
            {
                leftSql += ",";
                RawAttribute rawAttribute = rawAttributes[k];
                leftSql += $"({rawAttribute.query}) AS {rawAttribute.name}";
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
                if(index < includes.Length - 1)
                {
                    buildModel += ",";
                }
            }

            return buildModel;
        }

        public async Task<IEnumerable<Type>> QueryAsync<Type>(QueryAsyncParams queryAsyncParams) where Type : new()
        {
            string querySql = GetParentBuildModel(queryAsyncParams);

            Console.WriteLine(querySql);

            Dictionary<string, object> queryParams = queryAsyncParams.queryParams;

            IEnumerable<dynamic> result = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, queryParams);

            string jsonResult = GetJsonResult(result.ToArray());
            List<Type> data = JsonConvert.DeserializeObject<List<Type>>(jsonResult);
            return data.AsEnumerable();
        }

        public async Task<Type> QuerySingleOrDefaultAsync<Type>(QueryAsyncParams queryAsyncParams)
        {
            string querySql = GetParentBuildModel(queryAsyncParams);
            Console.WriteLine(querySql);
            Dictionary<string, object> queryParams = queryAsyncParams.queryParams;

            dynamic result = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<dynamic>(querySql, queryParams);
            string jsonResult = GetJsonResult(result);

            Console.WriteLine(jsonResult);

            return JsonConvert.DeserializeObject<Type>(jsonResult);
        }
    }
}