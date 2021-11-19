using System;
using System.Collections.Generic;
using System.Linq;

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
        public Attributes Attributes { get; set; }
        public string TableName { get; set; }
        public string Where { get; set; }
        public bool BelongsTo { get; set; }
        public List<IncludeModel> Includes { get; set; }
        public List<RawAttribute> RawAttributes { get; set; }

        public IncludeModel()
        {
            BelongsTo = false;
            Attributes = new Attributes();
            Includes = new List<IncludeModel>();
            RawAttributes = new List<RawAttribute>();
        }

        public string GetForeignKey()
        {
            return "";
        }

        public void ThenInclude<T>(IncludeModel IncludeModel)
        {
            IncludeModel.TableName = GetTableName<T>();
            List<string> IncludeAttributes = IncludeModel.Attributes.Include;
            List<string> ExcludeAttributes = IncludeModel.Attributes.Exclude;

            bool belongsTo = HasProperty<T>(IncludeModel.ForeignKey);
            IncludeModel.BelongsTo = belongsTo;

            if(!belongsTo) // Remover da consulta a FK
            {
                Attributes.Exclude.Add(IncludeModel.ForeignKey);
            }

            if(IncludeAttributes.Count == 0)
            {
                IncludeModel.Attributes.Include = GetAttributes<T>(ExcludeAttributes);
            }

            Includes.Add(IncludeModel);
            Slapper.AutoMapper.Configuration.AddIdentifier(typeof(T), "id");
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
