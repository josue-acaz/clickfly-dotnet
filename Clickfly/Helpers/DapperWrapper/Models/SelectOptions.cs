using System;
using System.Collections.Generic;
using System.Linq;

namespace clickfly.Data
{
    public class SelectOptions : QueryExtensions
    {
        public string PK { get; set; }
        public string As { get; set; } /*Opcional, caso não seja fornecido será usado o nome da classe e/ou somente o nome dos campos*/
        public string Code { get; set; }
        public string Query { get; set; }
        public string Where { get; set; }
        public string MainWhere { get; set; }
        public object Params { get; set; }
        public Attributes Attributes { get; set; }
        public List<IncludeModel> Includes { get; set; }
        public List<RawAttribute> RawAttributes { get; set; }
        public bool single { get; set; }

        public SelectOptions()
        {
            Attributes = new Attributes();
            Includes = new List<IncludeModel>();
            RawAttributes = new List<RawAttribute>();
        }

        internal void Include<T>(IncludeModel IncludeModel)
        {
            string pk = GetPK<T>();

            IncludeModel.PK = pk;
            IncludeModel.TableName = GetTableName<T>();
            string AsId = $"{IncludeModel.As}_id";

            // Verificar se é belongsTo (Se a ForeignKey reside no modelo)
            bool belongsTo = HasProperty<T>(IncludeModel.ForeignKey);
            IncludeModel.BelongsTo = belongsTo;

            List<string> IncludeAttributes = IncludeModel.Attributes.Include;
            List<string> ExcludeAttributes = IncludeModel.Attributes.Exclude;

            if(!belongsTo && AsId == IncludeModel.ForeignKey) // Remover da consulta a FK
            {
                Attributes.Exclude.Add(IncludeModel.ForeignKey); // Excluir da consulta pai
            }
            
            if(IncludeAttributes.Count == 0)
            {
                IncludeModel.Attributes.Include = GetAttributes<T>(ExcludeAttributes.ToList());
            }
            
            Includes.Add(IncludeModel);
            Slapper.AutoMapper.Configuration.AddIdentifier(typeof(T), pk);
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