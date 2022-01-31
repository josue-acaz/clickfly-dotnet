using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace clickfly.Data
{
    public class QueryExtensions
    {
        protected bool HasProperty<T>(string propertyName)
        {
            bool hasProperty = false;

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if(propertyInfo.Name == propertyName)
                {
                    hasProperty = true;
                }
            }

            return hasProperty;
        }

        protected bool ContainsProperty<T>(PropertyInfo propertyInfo)
        {
            bool contains = false;
            IEnumerable<Attribute> attributes = propertyInfo.GetCustomAttributes();
            
            foreach(Attribute attribute in attributes)
            {
                if(attribute is T)
                {
                    contains = true;
                }
            }

            return contains;
        }

        protected List<Property> GetProperties<Type>(List<string> exclude)
        {
            List<Property> properties = new List<Property>();
            PropertyInfo[] propertyInfos = typeof(Type).GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Property property = new Property();
                property.Name = propertyInfo.Name;
                property.Type = propertyInfo.PropertyType;

                bool isNotMapped = ContainsProperty<NotMappedAttribute>(propertyInfo);
                bool toExclude = exclude.FindIndex(ex => property.Name == ex) != -1;
                
                if(!isNotMapped && !toExclude)
                {
                    properties.Add(property);
                }
            }

            return properties;
        }

        protected List<string> GetAttributes<Type>(List<string> exclude)
        {   
            List<string> attributes = new List<string>();
            List<Property> properties = GetProperties<Type>(exclude);
            
            foreach (Property property in properties)
            {
                string propertyName = property.Name;
                var propertyType = property.Type;

                if(
                    propertyType == typeof(System.Int32) ||
                    propertyType == typeof(System.Int64) ||
                    propertyType == typeof(System.Boolean) ||
                    propertyType == typeof(System.Decimal) ||
                    propertyType == typeof(System.Single) ||
                    propertyType == typeof(System.String) ||
                    propertyType == typeof(System.DateTime) ||
                    propertyType == typeof(System.DateTime?) ||
                    propertyType == typeof(System.Boolean) ||
                    propertyType == typeof(System.Boolean?)
                )
                {
                    attributes.Add(propertyName);
                }
            }

            return attributes;
        }
    
        protected string GetPK<T>()
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                bool isPK = ContainsProperty<System.ComponentModel.DataAnnotations.KeyAttribute>(propertyInfo);

                if(isPK)
                {
                    return propertyInfo.Name;
                }
            }

            return "";
        }

        protected string GetTableName<T>()
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
            {
                return SqlMapperExtensions.TableNameMapper(typeof(T));
            }

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
            MethodInfo getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getTableNameMethod == null)
            {
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");
            }

            return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
        }
    
        protected string GetClassName<T>()
        {
            return typeof(T).Name;
        }
    
        protected string GetSelectAttributes(List<string> attributes, List<RawAttribute> rawAttributes, string As)
        {
            string attributesSql = $"";
            RawAttribute[] RawAttributes = rawAttributes.ToArray();

            for (int i = 0; i < attributes.Count; i++)
            {
                string attribute = attributes[i];
                attributesSql += $"{As}.{attribute}";
                bool isLastAttribute = i == attributes.Count -1;

                if(!isLastAttribute)
                {
                    attributesSql += ", ";
                }
            }

            for(int j = 0; j < RawAttributes.Length; j++)
            {
                attributesSql += ", ";
                RawAttribute rawAttribute = RawAttributes[j];
                string name = rawAttribute.Name;
                string query = rawAttribute.Query;

                attributesSql += $"({query}) AS {name}";
            }

            return attributesSql;
        }
    
        protected string GetSelectAttributesRecursive(List<IncludeModel> includeModels, string parentAs, bool nv1)
        {
            string attributesSql = $"";

            foreach (IncludeModel includeModel in includeModels)
            {
                attributesSql += ", ";
                string As = includeModel.As;
                string Code = includeModel.Code;
                string ForeignKey = includeModel.ForeignKey;
                string AsInclude = nv1 ? As : $"{parentAs}_{As}";
                List<string> attributes = includeModel.Attributes.Include;
                List<IncludeModel> thenIncludeModels = includeModel.Includes;
                RawAttribute[] rawAttributes = includeModel.RawAttributes.ToArray();

                for (int i = 0; i < attributes.Count; i++)
                {
                    string attribute = attributes[i];
                    attributesSql += $"{Code}.{attribute} AS {AsInclude}_{attribute}";
                    bool isLastAttribute = i == attributes.Count -1;
                    
                    if(!isLastAttribute)
                    {
                        attributesSql += ", ";
                    }
                }

                for (int j = 0; j < rawAttributes.Length; j++)
                {
                    attributesSql += ", ";
                    RawAttribute rawAttribute = rawAttributes[j];
                    string name = rawAttribute.Name;
                    string query = rawAttribute.Query;

                    attributesSql += $"({query}) AS {AsInclude}_{name}";
                }

                attributesSql += GetSelectAttributesRecursive(thenIncludeModels, AsInclude, false);
            }

            return attributesSql;
        }
    
        protected string IncludeModelsToQuery(string parentPK, List<IncludeModel> includeModels, string parentAs, string parentIncludeSql)
        {
            string includeSql = $"";
            foreach (IncludeModel includeModel in includeModels)
            {
                string pk = includeModel.PK;
                string AS = includeModel.As;
                string Code = includeModel.Code;
                string tableName = includeModel.TableName;
                string foreignKey = includeModel.ForeignKey;
                string where = includeModel.Where;
                bool belongsTo = includeModel.BelongsTo;
                List<string> attributes = includeModel.Attributes.Include;
                List<RawAttribute> rawAttributes = includeModel.RawAttributes;

                string joinStatementSql = $"{parentAs}.{foreignKey} = {Code}.{parentPK}";
                if(belongsTo)
                {
                    joinStatementSql = $"{parentAs}.{parentPK} = {Code}.{foreignKey}";
                }

                includeSql += $"\nLEFT JOIN (\n\tSELECT * FROM {tableName} AS {Code}";//{GetSelectAttributes(attributes, rawAttributes, Code)}
                if(where != null)
                {
                    includeSql += $" WHERE {where} ";
                }
                
                includeSql += $"\n) AS {Code} ON {joinStatementSql}\n";
                includeSql += IncludeModelsToQuery(pk, includeModel.Includes, Code, parentIncludeSql);
                parentIncludeSql += includeSql;
            }

            return includeSql;
        }

        protected string ReplaceQuery(string Query, string As, string Code, List<string> excludedKeys = null)
        {
            return Query.Replace($"{As}.", $"{Code}.");
        }

        protected string ReplaceQueryRecursive(string Query, List<IncludeModel> includes, List<string> excludedKeys = null)
        {
            for (int i = 0; i < includes.Count; i++)
            {
                Query = ReplaceQuery(Query, includes[i].As, includes[i].Code, excludedKeys);
                if(includes[i].Includes.Count > 0)
                {
                    Query = ReplaceQueryRecursive(Query, includes[i].Includes, excludedKeys);
                }
            }
            
            return Query;
        }

        protected string ReplaceQueryRecursive(string Query, List<IncludeModel> includes, string As, string Code)
        {
            for (int i = 0; i < includes.Count; i++)
            {
                Query = ReplaceQuery(Query, As, Code);
                if(includes[i].Includes.Count > 0)
                {
                    Query = ReplaceQueryRecursive(Query, includes[i].Includes, As, Code);
                }
            }
            
            return Query;
        }

        protected List<RawAttribute> ReplaceRaw(List<RawAttribute> rawAttributes, string As, string Code)
        {
            for (int i = 0; i < rawAttributes.Count; i++)
            {
                rawAttributes[i].Query = ReplaceQuery(rawAttributes[i].Query, As, Code);
            }

            return rawAttributes;
        }

        protected List<RawAttribute> ReplaceRaw(List<RawAttribute> rawAttributes, List<IncludeModel> includeModels)
        {
            for (int i = 0; i < rawAttributes.Count; i++)
            {
                rawAttributes[i].Query = ReplaceQueryRecursive(rawAttributes[i].Query, includeModels);
            }

            return rawAttributes;
        }

        protected List<RawAttribute> ReplaceRaw(List<RawAttribute> rawAttributes, List<IncludeModel> includeModels, string As, string Code)
        {
            for (int i = 0; i < rawAttributes.Count; i++)
            {
                rawAttributes[i].Query = ReplaceQueryRecursive(rawAttributes[i].Query, includeModels, As, Code);
            }

            return rawAttributes;
        }

        protected List<IncludeModel> ReplaceIncludesRecursive(List<IncludeModel> includeModels, string As, string Code)
        {
            for (int i = 0; i < includeModels.Count; i++)
            {
                IncludeModel include = includeModels[i];
                include.RawAttributes = ReplaceRaw(include.RawAttributes, includeModels, As, Code);

                if(include.Includes.Count > 0)
                {
                    include.Includes = ReplaceIncludesRecursive(include.Includes, As, Code);
                }

                includeModels[i] = include;
            }

            return includeModels;
        }

        protected List<IncludeModel> ReplaceIncludesRecursive(List<IncludeModel> includeModels)
        {
            for (int i = 0; i < includeModels.Count; i++)
            {
                IncludeModel include = includeModels[i];
                include.RawAttributes = ReplaceRaw(include.RawAttributes, includeModels);

                if(include.Includes.Count > 0)
                {
                    include.Includes = ReplaceIncludesRecursive(include.Includes);
                }

                includeModels[i] = include;
            }

            return includeModels;
        }

        protected int CountIncludeRecursive(List<IncludeModel> includeModels, int index)
        {
            int count = index;
            for (int i = 0; i < includeModels.Count; i++)
            {
                IncludeModel include = includeModels[i];
                List<IncludeModel> thenIncludes = include.Includes;

                string Code = $"t{count}";
                string Where = include.Where;

                include.Code = Code;
                if(include.Where != null && include.Where != "")
                {
                    include.Where = Where.Replace(include.As, Code);
                }
                count += 1;

                if(thenIncludes.Count > 0)
                {
                    count = CountIncludeRecursive(thenIncludes, count);
                }
            }

            return count;
        }
    
        protected string GetSelectQuery(SelectQueryParams selectQueryParams)
        {
            string Code = "t";
            string pk = selectQueryParams.PK;
            string As = selectQueryParams.As;
            bool single = selectQueryParams.single;
            string tableName = selectQueryParams.TableName;
            List<string> attributes = selectQueryParams.Attributes;
            string Where = selectQueryParams.Where;
            string MainWhere = selectQueryParams.MainWhere;
            List<IncludeModel> includeModels = selectQueryParams.Includes;
            List<string> MappingExcludeKeys = new List<string>();

            if(selectQueryParams.Params != null)
            {
                IDictionary<string, object> queryParams = selectQueryParams.Params.ToDictionary();
                foreach(KeyValuePair<string, object> kvp in queryParams)
                {
                    MappingExcludeKeys.Add($"@{kvp.Key}");
                }
            }

            // Mapping all tables
            int count = CountIncludeRecursive(includeModels, 0);

            // Replace raw attributes from main query
            List<RawAttribute> rawAttributes = ReplaceRaw(selectQueryParams.RawAttributes, As, Code); // Main
            rawAttributes = ReplaceRaw(rawAttributes, includeModels); // Includes

            // Replace raw attributes from includes
            // Replace where in "CountIncludeRecursive"
            includeModels = ReplaceIncludesRecursive(includeModels, As, Code); // Main
            includeModels = ReplaceIncludesRecursive(includeModels); // Includes

            // Default Attributes For Main Model
            string attributesSql = GetSelectAttributes(attributes, rawAttributes, Code);

            // Attributes For Include Models
            if(includeModels.Count > 0)
            {
                attributesSql += GetSelectAttributesRecursive(includeModels, Code, true);
            }

            string querySql = $"\nSELECT {attributesSql} FROM (SELECT * FROM {tableName} AS {Code}";
            if(Where != null && Where != "")
            {
                Where = ReplaceQuery(Where, As, Code, MappingExcludeKeys);
                querySql += $"\nWHERE {Where}";
            }
            if(single)
            {
                querySql += "\nLIMIT 1";
            }

            querySql += $") AS {Code}\n";
            querySql += IncludeModelsToQuery(pk, includeModels, Code, querySql);

            if(MainWhere != null && MainWhere != "")
            {
                MainWhere = ReplaceQuery(MainWhere, As, Code, MappingExcludeKeys);
                MainWhere = ReplaceQueryRecursive(MainWhere, includeModels, MappingExcludeKeys);
                querySql += $"\nWHERE {MainWhere}\n";
            }

            return querySql;
        }
    
        protected string GetUpdateAttributes(List<string> attributes)
        {
            string attributesSql = $" ";
            for (int i = 0; i < attributes.Count; i++)
            {
                string attribute = attributes[i];
                
                if(i != 0)
                {
                    attributesSql += ",";
                }
                attributesSql += $"{attribute} = @{attribute}";
            }
            attributesSql += " ";

            return attributesSql;
        }

        protected string GetUpdateQuery<T>(UpdateQueryParams updateQueryParams)
        {
            string pk = GetPK<T>();
            string tableName = GetTableName<T>();
            string whereSql = updateQueryParams.Where;

            List<string> exclude = updateQueryParams.Exclude;
            exclude.Add(pk);

            List<string> attributes = GetAttributes<T>(exclude);

            string attributesSql = GetUpdateAttributes(attributes);
            string querySql = $"UPDATE {tableName} SET {attributesSql}";

            if(whereSql != null)
            {
                querySql += $" WHERE {whereSql} ";
            }

            return querySql;
        }
    
        protected string GetInsertAttributes(List<string> attributes)
        {
            string attributesSql = $"";
            for (int i = 0; i < attributes.Count; i++)
            {
                string attribute = attributes[i];
                
                if(i != 0)
                {
                    attributesSql += ",";
                }
                attributesSql += attribute;
            }

            string valuesSql = $"";
            for (int i = 0; i < attributes.Count; i++)
            {
                string attribute = attributes[i];
                
                if(i != 0)
                {
                    valuesSql += ",";
                }
                valuesSql += $"@{attribute}";
            }

            attributesSql = $"({attributesSql}) VALUES({valuesSql})";
            return attributesSql;
        }

        protected string GetInsertQuery<T>(InsertQueryParams insertQueryParams)
        {
            List<string> exclude = insertQueryParams.Exclude;

            string tableName = GetTableName<T>();
            List<string> attributes = GetAttributes<T>(exclude);

            string attributesSql = GetInsertAttributes(attributes);
            string querySql = $"INSERT INTO {tableName}{attributesSql}";
            
            return querySql;
        }
    
        protected string GetSelectQuery<T>(SelectOptions options)
        {
            string Pk = GetPK<T>();
            string As = options.As;
            string Code = options.Code;
            string Where = options.Where;
            object Params = options.Params;
            string MainWhere = options.MainWhere;
            object optionParams = options.Params;
            List<IncludeModel> optionIncludes = options.Includes;

            if(As == null)
            {
                As = GetClassName<T>();
            }

            string tableName = GetTableName<T>();
            List<string> defaultAttributes = options.Attributes.Include.Count > 0 ? options.Attributes.Include : GetAttributes<T>(options.Attributes.Exclude);
            List<RawAttribute> rawAttributes = options.RawAttributes;

            SelectQueryParams queryParams = new SelectQueryParams();
            queryParams.TableName = tableName;
            queryParams.Attributes = defaultAttributes;
            queryParams.RawAttributes = rawAttributes;
            queryParams.Includes = optionIncludes;
            queryParams.single = options.single;
            queryParams.MainWhere = MainWhere;
            queryParams.Params = Params;
            queryParams.Where = Where;
            queryParams.Code = Code;
            queryParams.As = As;
            queryParams.PK = Pk;

            string querySql = GetSelectQuery(queryParams);
            return querySql;
        }
    }
};