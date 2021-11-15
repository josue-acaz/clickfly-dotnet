using System;
using System.Collections.Generic;

namespace clickfly.ViewModels
{
    public class RawAttribute
    {
        public string Name { get; set; }
        public string Query { get; set; }
    }

    public class IncludeModel : QueryExtensions {
        public string As { get; set; }
        public string ForeignKey { get; set; }
        public string[] Attributes { get; set; }
        public string TableName { get; set; }
        public string Where { get; set; }
        public List<IncludeModel> Includes { get; set; }
        public List<RawAttribute> RawAttributes { get; set; }

        public IncludeModel()
        {
            Attributes = new string[0];
            Includes = new List<IncludeModel>();
            RawAttributes = new List<RawAttribute>();
        }

        public void ThenInclude<T>(IncludeModel IncludeModel)
        {
            IncludeModel.TableName = GetTableName<T>();

            if(IncludeModel.Attributes.Length == 0)
            {
                IncludeModel.Attributes = GetAttributes<T>();
            }

            Includes.Add(IncludeModel);
        }

        public void AddRawAttribute(string Name, string Query)
        {
            RawAttribute rawAttribute = new RawAttribute();
            rawAttribute.Name = Name;
            rawAttribute.Query = Query;

            RawAttributes.Add(rawAttribute);
        }
    }
}
