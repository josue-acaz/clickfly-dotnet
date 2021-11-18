using System;
using System.Collections.Generic;
using System.Linq;

namespace clickfly.ViewModels
{
    public class QueryOptions : QueryExtensions {
        public string As { get; set; } /*Opcional, caso não seja fornecido será usado o nome da classe*/
        public string Query { get; set; }
        public string Where { get; set; }
        public object Params { get; set; }
        public Attributes Attributes { get; set; }
        public List<IncludeModel> Includes { get; set; }
        public List<RawAttribute> RawAttributes { get; set; }

        public QueryOptions()
        {
            Attributes = new Attributes();
            Includes = new List<IncludeModel>();
            RawAttributes = new List<RawAttribute>();
        }

        internal void Include<T>(IncludeModel IncludeModel)
        {
            IncludeModel.TableName = GetTableName<T>();

            List<string> IncludeAttributes = IncludeModel.Attributes.Include;
            List<string> ExcludeAttributes = IncludeModel.Attributes.Exclude;
            
            if(IncludeAttributes.Count == 0)
            {
                IncludeModel.Attributes.Include = GetAttributes<T>(ExcludeAttributes.ToList());
            }
            
            Includes.Add(IncludeModel);
            Slapper.AutoMapper.Configuration.AddIdentifier(typeof(T), "id");
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
