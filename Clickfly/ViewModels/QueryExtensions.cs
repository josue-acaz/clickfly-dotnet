using System;
using System.Reflection;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using clickfly.Models;
using Newtonsoft.Json;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.ViewModels
{
    public class QueryExtensions
    {
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

        protected List<Property> GetProperties<Type>()
        {
            List<Property> properties = new List<Property>();
            PropertyInfo[] propertyInfos = typeof(Type).GetProperties();

            List<string> foreignKeys = new List<string>();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Property property = new Property();
                property.Name = propertyInfo.Name;
                property.Type = propertyInfo.PropertyType;

                bool isNotMapped = ContainsProperty<NotMappedAttribute>(propertyInfo);
                bool isForeignKey = ContainsProperty<ForeignKeyAttribute>(propertyInfo);
                
                bool notForeignKey = false;
                if(isForeignKey)
                {
                    ForeignKeyAttribute foreignKeyAttribute = propertyInfo.GetCustomAttribute<ForeignKeyAttribute>();
                    if(foreignKeyAttribute != null)
                    {
                        string foreignKeyName = foreignKeyAttribute.Name;
                        foreignKeys.Add(foreignKeyName);

                        int propertyIndexFK = properties.FindIndex(PR => PR.Name == foreignKeyName);
                        notForeignKey = propertyIndexFK == -1;
                        if(!notForeignKey)
                        {
                            properties.RemoveAt(propertyIndexFK);
                        }
                    }
                }

                if(!isNotMapped && !isForeignKey)
                {
                    notForeignKey = foreignKeys.FindIndex(FK => property.Name == FK) == -1;
                    if(notForeignKey)
                    {
                        properties.Add(property);
                    }
                }
            }

            return properties;
        }

        protected List<string> GetAttributes<Type>(List<string> exclude)
        {   
            List<string> attributes = new List<string>();
            List<Property> properties = GetProperties<Type>();
            
            foreach (Property property in properties)
            {
                string propertyName = property.Name;
                var propertyType = property.Type;

                if(
                    propertyType == typeof(System.Int32) ||
                    propertyType == typeof(System.String) ||
                    propertyType == typeof(System.DateTime) ||
                    propertyType == typeof(System.DateTime?) ||
                    propertyType == typeof(System.Boolean) ||
                    propertyType == typeof(System.Boolean?)
                )
                {
                    bool toExclude = exclude.FindIndex(ex => propertyName == ex) != -1;
                    
                    if(!toExclude)
                    {
                        attributes.Add(propertyName);
                    }
                }
            }

            return attributes;
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

        protected string GetAttributesSql(List<string> attributes, List<RawAttribute> rawAttributes, string As)
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

        protected string GetAttributesSql(List<IncludeModel> includeModels, string parentAs, bool nv1)
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

                attributesSql += GetAttributesSql(thenIncludeModels, AsInclude, false);
            }

            return attributesSql;
        }
        
        protected string IncludeModelsToQuery(List<IncludeModel> includeModels, string parentAs, string parentIncludeSql)
        {
            string includeSql = $"";
            foreach (IncludeModel includeModel in includeModels)
            {
                string AS = includeModel.As;
                string tableName = includeModel.TableName;
                string foreignKey = includeModel.ForeignKey;
                bool belongsTo = includeModel.BelongsTo;

                bool alreadyIncluded = parentIncludeSql.Contains($"{tableName} AS {AS}");
                if(!alreadyIncluded)
                {
                    string joinStatementSql = $"{parentAs}.{foreignKey} = {AS}.id";

                    if(belongsTo)
                    {
                        joinStatementSql = $"{parentAs}.id = {AS}.{foreignKey}";
                    }

                    includeSql += $" LEFT OUTER JOIN {tableName} AS {AS} ON {joinStatementSql} ";
                }

                includeSql += IncludeModelsToQuery(includeModel.Includes, AS, parentIncludeSql);
                parentIncludeSql += includeSql;
            }

            return includeSql;
        }
        
        protected string CreateQuery(CreateQueryParams createQueryParams)
        {
            string As = createQueryParams.As;
            string tableName = createQueryParams.TableName;
            List<string> attributes = createQueryParams.Attributes;
            string whereSql = createQueryParams.Where;
            List<IncludeModel> includeModels = createQueryParams.Includes;
            List<RawAttribute> rawAttributes = createQueryParams.RawAttributes;

            // Default Attributes For Main Model
            string attributesSql = GetAttributesSql(attributes, rawAttributes, As);

            // Attributes For Include Models
            if(includeModels.Count > 0)
            {
                attributesSql += GetAttributesSql(includeModels, As, true);
            }

            string querySql = $" SELECT {attributesSql} FROM {tableName} AS {As} ";
            querySql += IncludeModelsToQuery(includeModels, As, querySql);

            if(whereSql != null)
            {
                querySql += $" WHERE {whereSql} ";
            }

            return querySql;
        }
    }
}

/*
protected string GetAttributesSql(List<IncludeModel> includeModels, string parentAs, bool nv1)
        {
            string attributesSql = $"";

            foreach (IncludeModel includeModel in includeModels)
            {
                attributesSql += ", ";
                string As = includeModel.As;
                string AsInclude = nv1 ? As : $"{parentAs}.{As}";
                string[] attributes = includeModel.Attributes;
                List<IncludeModel> thenIncludeModels = includeModel.Includes;
                RawAttribute[] rawAttributes = includeModel.RawAttributes.ToArray();

                for (int i = 0; i < attributes.Length; i++)
                {
                    string attribute = attributes[i];
                    attributesSql += $"{As}.{attribute} AS \"{AsInclude}.{attribute}\"";
                    bool isLastAttribute = i == attributes.Length -1;

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

                    attributesSql += $"({query}) AS \"{AsInclude}.{name}\"";
                }

                attributesSql += GetAttributesSql(thenIncludeModels, AsInclude, false);
            }

            return attributesSql;
        }
*/