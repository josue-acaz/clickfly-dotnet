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

            // Verificar se existe confronto da alias com o id da table
            // Por exemplo, a table Passenger, tem o alias "passenger" o que fica "passenger_id" em uma relacionamento, o que iria gerar um conflito com a FK
            // Já a tabela AircraftModel tem o alias "model", isso fica "model_id" o que não irá gerar conflito na tabela, e consequentemente a FK da tabela pai virá null pela regra de remoção da FK
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
