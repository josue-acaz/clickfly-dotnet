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
                string AsInclude = nv1 ? As : $"{parentAs}_{As}";
                List<string> attributes = includeModel.Attributes.Include;
                List<IncludeModel> thenIncludeModels = includeModel.Includes;
                RawAttribute[] rawAttributes = includeModel.RawAttributes.ToArray();

                for (int i = 0; i < attributes.Count; i++)
                {
                    string attribute = attributes[i];
                    attributesSql += $"{As}.{attribute} AS {AsInclude}_{attribute}";
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
                string tableName = includeModel.TableName;
                string foreignKey = includeModel.ForeignKey;
                string where = includeModel.Where;
                bool belongsTo = includeModel.BelongsTo;

                bool alreadyIncluded = parentIncludeSql.Contains($"{tableName} AS {AS}");
                if(!alreadyIncluded)
                {
                    string joinStatementSql = $"{parentAs}.{foreignKey} = {AS}.{parentPK}";

                    if(belongsTo)
                    {
                        joinStatementSql = $"{parentAs}.{parentPK} = {AS}.{foreignKey}";
                    }

                    includeSql += $" LEFT OUTER JOIN (SELECT * FROM {tableName} AS {AS}";
                    if(where != null)
                    {
                        includeSql += $" WHERE {where} ";
                    }
                    includeSql += $") {AS} ON {joinStatementSql} ";
                }

                includeSql += IncludeModelsToQuery(pk, includeModel.Includes, AS, parentIncludeSql);
                parentIncludeSql += includeSql;
            }

            return includeSql;
        }
    
        protected string GetSelectQuery(SelectQueryParams selectQueryParams)
        {
            string pk = selectQueryParams.PK;
            string As = selectQueryParams.As;
            string tableName = selectQueryParams.TableName;
            List<string> attributes = selectQueryParams.Attributes;
            string whereSql = selectQueryParams.Where;
            List<IncludeModel> includeModels = selectQueryParams.Includes;
            List<RawAttribute> rawAttributes = selectQueryParams.RawAttributes;

            // Default Attributes For Main Model
            string attributesSql = GetSelectAttributes(attributes, rawAttributes, As);

            // Attributes For Include Models
            if(includeModels.Count > 0)
            {
                attributesSql += GetSelectAttributesRecursive(includeModels, As, true);
            }

            string querySql = $" SELECT {attributesSql} FROM {tableName} AS {As} ";
            querySql += IncludeModelsToQuery(pk, includeModels, As, querySql);

            if(whereSql != null)
            {
                querySql += $" WHERE {whereSql} ";
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
            string where = options.Where;
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
            queryParams.Where = where;
            queryParams.As = As;
            queryParams.PK = Pk;

            string querySql = GetSelectQuery(queryParams);
            return querySql;
        }
    }
};