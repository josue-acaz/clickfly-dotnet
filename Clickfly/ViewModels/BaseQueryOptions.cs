using System;
using System.Collections.Generic;

namespace clickfly.ViewModels
{
    public class QueryOptions : QueryExtensions {
        public string As { get; set; }
        public string Query { get; set; }
        public string Where { get; set; }
        public object Params { get; set; }
        public string[] Attributes { get; set; }
        public List<IncludeModel> Includes { get; set; }
        public List<RawAttribute> RawAttributes { get; set; }

        public QueryOptions()
        {
            Attributes = new string[0];
            Includes = new List<IncludeModel>();
            RawAttributes = new List<RawAttribute>();
        }

        internal void Include<T>(IncludeModel IncludeModel)
        {
            IncludeModel.TableName = GetTableName<T>();
            
            if(IncludeModel.Attributes.Length == 0)
            {
                IncludeModel.Attributes = GetAttributes<T>();
            }
            
            Includes.Add(IncludeModel);
        }

        internal void AddRawAttribute(string Name, string Query)
        {
            RawAttribute rawAttribute = new RawAttribute();
            rawAttribute.Name = Name;
            rawAttribute.Query = Query;

            RawAttributes.Add(rawAttribute);
        }
    }
}
