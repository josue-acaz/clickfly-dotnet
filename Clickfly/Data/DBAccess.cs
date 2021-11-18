using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;
using Dapper;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace clickfly.Data
{
    public class DBAccess : QueryExtensions, IDBAccess
    {
        private readonly IDBContext _dBContext;
        private readonly IDataContext _dataContext;
        
        public DBAccess(IDBContext dBContext, IDataContext dataContext)
        {
            _dBContext = dBContext;
            _dataContext = dataContext;
        }

        internal string GetQuery<T>(QueryOptions queryOptions)
        {
            string As = queryOptions.As;
            string where = queryOptions.Where;
            object optionParams = queryOptions.Params;
            List<IncludeModel> optionIncludes = queryOptions.Includes;

            if(As == null)
            {
                As = GetClassName<T>();
            }

            string tableName = GetTableName<T>();
            List<string> defaultAttributes = queryOptions.Attributes.Include.Count > 0 ? queryOptions.Attributes.Include : GetAttributes<T>(queryOptions.Attributes.Exclude);
            List<RawAttribute> rawAttributes = queryOptions.RawAttributes;

            CreateQueryParams createQueryParams = new CreateQueryParams();
            createQueryParams.TableName = tableName;
            createQueryParams.Attributes = defaultAttributes;
            createQueryParams.RawAttributes = rawAttributes;
            createQueryParams.Includes = optionIncludes;
            createQueryParams.Where = where;
            createQueryParams.As = As;

            string querySql = CreateQuery(createQueryParams);
            return querySql;
        }

        internal T SerializeResult<T>(dynamic result)
        {
            string queryResultJSON = JsonConvert.SerializeObject(result);
            queryResultJSON = queryResultJSON.Replace($"DapperRow, ", "");

            DefaultContractResolver resolver = new DefaultContractResolver();
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = resolver, 
                Converters = new JsonConverter[] { 
                    new JsonFlatteningConverter(resolver)
                }
            };

            return(JsonConvert.DeserializeObject<T>(queryResultJSON, settings));
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(QueryOptions options)
        {
            string querySql = GetQuery<T>(options);
            dynamic queryResult = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, options.Params);
            return(SerializeResult<IEnumerable<T>>(queryResult));
        }
    
        public async Task<T> QuerySingleAsync<T>(QueryOptions options)
        {/*Excluir foreignKey do mapeamento, o Slapper.Aumapper Crasha se por exemplo for mapeador ticker.passenger_id e passenger.id As passenger_id, já que teremos dois identificadores iguais passenger_id = passenger_id*/
            QueryOptions queryOptions = options;
            string querySql = GetQuery<T>(queryOptions);

            Console.WriteLine(querySql);

            IEnumerable<dynamic> queryResult = await _dBContext.GetConnection().QueryAsync<dynamic>(querySql, options.Params);
            
            Slapper.AutoMapper.Configuration.AddIdentifier(typeof(T), "id");
            IEnumerable<T> queryResultInstance = Slapper.AutoMapper.MapDynamic<T>(queryResult);
            
            return queryResultInstance.FirstOrDefault<T>();
        }

        public bool AlreadyAdded(Dictionary<string, object> includeModel, dynamic targetId)
        {
            bool alreadyAdded = false;

            foreach(KeyValuePair<string, object> kvp in includeModel)
            {
                if(kvp.Key == "id")
                {
                    if(kvp.Value == targetId)
                    {
                        alreadyAdded = true;
                    }
                }
            }

            return alreadyAdded;
        }

        /*
        dynamic obj = await _dBContext.GetConnection().QueryAsync<dynamic>($@"
                SELECT	customer.id,
                    customer.name,
                    customer.email,
                    ({customerThumbnailSql}) As thumbnail,
                    o.id AS cards_id,
                    o.card_id AS cards_card_id,
                    a.id AS addresses_id,
                    a.name AS addresses_name,
                    a.street As addresses_street,
                    a.state AS addresses_state
            FROM	customers customer
                    LEFT OUTER JOIN customer_cards o ON customer.id = o.customer_id
                    LEFT OUTER JOIN customer_addresses a ON customer.id = a.customer_id
                    WHERE customer.id = @id
            ", new { id = id });

            Slapper.AutoMapper.Configuration.AddIdentifier(
                    typeof(Customer), "id");
            Slapper.AutoMapper.Configuration.AddIdentifier(
                typeof(CustomerCard), "id");
                Slapper.AutoMapper.Configuration.AddIdentifier(typeof(CustomerAddress), "id");

            IEnumerable<Customer> customer = Slapper.AutoMapper.MapDynamic<Customer>(obj);

            Console.WriteLine("Tamanho do array: " + customer.ToArray().Length);

            foreach(var c in customer)
            {
                Console.WriteLine(c);
            }

            return customer.FirstOrDefault();
        */

        /*public void ConfigQuery<T>(IEnumerable<dynamic> queryResult)
        {
            string queryResultJSON = JsonConvert.SerializeObject(queryResult);
            queryResultJSON = queryResultJSON.Replace("DapperRow, ", "");

            dynamic[] result = JsonConvert.DeserializeObject<dynamic[]>(queryResultJSON);
            
            // Executar esse algoritimo quando o tipo de consulta é conhecido
            // Por exemplo.: Um cliente possui muitos endereços, nesse caso, a consulta irá retornar uma quantidade de linhas de acordo com a quantidade de endereços
            // Nesse caso, será preciso mapear esse resultado para somente uma estrutura e colocar dentro de um array os endereços
            

                -- RESULTADO DA CONSULTA
                Cliente 1, Endereço 1
                Cliente 1, Endereço 2
                Cliente 1, Endereço 3

                -- SAÍDA ESPERADA
                Cliente 1, Endereços: [
                    Endereço 1,
                    Endereço 2,
                    Endereço 3
                ]

            Dictionary<string, dynamic> model = new Dictionary<string, dynamic>();
            List<Dictionary<string, object>> includes = new List<Dictionary<string, object>>();
            
            for (int i = 0; i < result.Length; i++)
            {
                dynamic row = result[i];
                bool isFirstRow = i == 0;

                string LastPropertyAs = $"";
                Dictionary<string, object> includeModel = new Dictionary<string, object>();

                bool alreadyAdded = false;
                foreach(JProperty jProperty in row)
                {
                    string Key = jProperty.Name;
                    var Value = jProperty.First;

                    if(Key.Contains("."))
                    {
                        string[] Property = Key.Split(".");

                        string PropertyAs = Property[0];
                        var PropertyKey = Property[1];

                        if(PropertyKey == "id")
                        {
                            alreadyAdded = includes.FindIndex(includeModel => AlreadyAdded(includeModel, Value)) != -1;
                            Console.WriteLine(alreadyAdded);
                        }

                        if(PropertyAs != LastPropertyAs && !alreadyAdded)
                        {
                            includes.Add(includeModel);
                            includeModel.Clear();
                        }

                        if(!alreadyAdded)
                        {
                            includeModel.Add(PropertyKey, Value);
                        }

                        LastPropertyAs = PropertyAs;
                    }
                    else
                    {
                        if(isFirstRow)
                        {
                            model.Add(Key, Value);
                        }
                    }
                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(includes));

            /*
            string LastKey = "";
                dynamic row = result[i];
                foreach(JProperty jProperty in row)
                {
                    string Key = jProperty.Name;
                    var Value = jProperty.First;

                    if(Key.Contains(".")) // Colocar no array
                    {
                        string[] AsProperty = Key.Split(".");
                        string[] LastAsProperty = LastKey.Split(".");
                        
                        string As = AsProperty[0];
                        string Property = AsProperty[1];

                        string LastAs = LastAsProperty.Length > 1 ? LastAsProperty[0] : "";
                        string LastProperty = LastAsProperty.Length > 1 ? LastAsProperty[1] : "";

                        if(As != LastAs && LastKey != "")
                        {
                            Console.WriteLine("\n\n\n\nAQUIII\n\n\n\n");
                            includes.Add(includeModel);

                            includeModel.Clear();
                            includeModel.Add(Property, Value);
                        }
                        else
                        {
                            Console.WriteLine($"Key: {Property}, Value: {Value}");
                            includeModel.Add(Property, Value);
                        }

                        LastKey = Key;
                    }
                    else // É o modelo principal, colocar somente a primeira iterção
                    {
                        if(i == 0)
                        {
                            model.Add(Key, Value);
                        }
                    }
                }
            
        }*/
    }
}
