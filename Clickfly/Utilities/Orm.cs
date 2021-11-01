using System;
using System.Collections.Generic;
using System.Data;
using clickfly.ViewModels;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.Reflection;

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

        public string GetBuildModel(string buildModel, Include parent, Include[] includes)
        {
            for (int index = 0; index < includes.Length; index++)
            {
                var include = includes[index];
                var thenIncludes = include.includes.ToArray();
                string[] attributes = include.attributes;

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
                        buildModel = GetBuildModel(buildModel, include, thenIncludes);
                    }
                }

                buildModel += $") FROM {include.tableName} AS {include.relationshipName} WHERE {parent.relationshipName}.{include.foreignKey} = {include.relationshipName}.id)";
            }

            return buildModel;
        }

        public async Task<IEnumerable<Type>> QueryAsync<Type>(QueryAsyncParams queryAsyncParams) where Type : new()
        {
            string querySql = queryAsyncParams.querySql;
            string tableName = queryAsyncParams.tableName;
            string relationshipName = queryAsyncParams.relationshipName;
            string foreignKey = queryAsyncParams.foreignKey;
            Dictionary<string, object> queryParams = queryAsyncParams.queryParams;

            var includes = queryAsyncParams.includes.ToArray();
            string[] queryIndex = querySql.Split(new[] { '?' }, 2);

            string leftSql = queryIndex[0];
            string rightSql = queryIndex[1];

            Console.WriteLine(leftSql);
            Console.WriteLine(rightSql);

            leftSql += ", ";
            for (int i = 0; i < includes.Length; i++)
            {
                var include = includes[i];
                string[] attributes = include.attributes;

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
                        buildModel = GetBuildModel(buildModel, include, thenIncludes);
                    }
                }

                buildModel += $") FROM {include.tableName} AS {include.relationshipName} WHERE {relationshipName}.{include.foreignKey} = {include.relationshipName}.id";
                leftSql += $"({buildModel}) AS {include.relationshipName}";
                
                if(i < includes.Length - 1)
                {
                    leftSql += ",";
                }
            }

            string _querySql = $"SELECT * FROM ({leftSql} FROM {rightSql}) AS {relationshipName}";    
            
            Console.WriteLine(_querySql);
            IEnumerable<dynamic> result = await _dBContext.GetConnection().QueryAsync<dynamic>(_querySql, queryParams);
            
            dynamic[] _result = result.ToArray();
            List<Type> __result = new List<Type>();

            for (int i = 0; i < _result.Length; i++)
            {
                dynamic model = _result[i];
                Type modelInstance = new Type();

                foreach(KeyValuePair<string, object> kvp in model)
                {
                    var includeIndex = queryAsyncParams.includes.FindIndex(x => x.relationshipName == kvp.Key);
                    var prop = modelInstance.GetType().GetProperty(kvp.Key);
                    bool isRelationship = includeIndex != -1;

                    if(isRelationship)
                    {
                        var include = queryAsyncParams.includes[includeIndex];
                        string value = kvp.Value.ToString();

                        if(include.tableName == "aircrafts")
                        {
                            prop.SetValue(modelInstance, JsonSerializer.Deserialize<Aircraft>(value));
                        }
                        if(include.tableName == "aerodromes")
                        {
                            prop.SetValue(modelInstance, JsonSerializer.Deserialize<Aerodrome>(value));
                        }
                        if(include.tableName == "flights")
                        {
                            prop.SetValue(modelInstance, JsonSerializer.Deserialize<Flight>(value));
                        }
                        if(include.tableName == "flight_segments")
                        {
                            prop.SetValue(modelInstance, JsonSerializer.Deserialize<FlightSegment>(value));
                        }
                    }
                    else
                    {
                        prop.SetValue(modelInstance, kvp.Value);
                    }
                }

                __result.Add(modelInstance);
            }

            return __result.AsEnumerable();
        }
    }
}