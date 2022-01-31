using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;
using Dapper;

namespace clickfly.Repositories
{
    public class AerodromeRepository : BaseRepository<Aerodrome>, IAerodromeRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "aerodromes as aerodrome";
        private static string whereSql = "aerodrome.excluded = false";
        private static string deleteSql = "UPDATE aerodromes SET excluded = true WHERE id = @id";

        public AerodromeRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Aerodrome> Create(Aerodrome aerodrome)
        {
            aerodrome.id = Guid.NewGuid().ToString();
            aerodrome.created_at = DateTime.Now;
            aerodrome.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = aerodrome;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Aerodrome>(options);
            return aerodrome;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public async Task<Aerodrome> GetById(string id)
        {
            SelectOptions options = new SelectOptions();
            options.As = "aerodrome";
            options.Where = $"{whereSql} AND aerodrome.id = @id";
            options.Params = new { id = id };

            IncludeModel includeCity = new IncludeModel();
            includeCity.As = "city";
            includeCity.ForeignKey = "city_id";
            includeCity.ThenInclude<State>(new IncludeModel{
                As = "state",
                ForeignKey = "state_id",
            });

            options.Include<City>(includeCity);

            Aerodrome aerodrome = await _dapperWrapper.QuerySingleAsync<Aerodrome>(options);
            
            return aerodrome;
        }

        public async Task<PaginationResult<Aerodrome>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string text = filter.text;

            Console.WriteLine("Pesquisando...");
            Console.WriteLine(text);

            string where = $"{whereSql} AND aerodrome.name ILIKE @text OR aerodrome.oaci_code ILIKE @text LIMIT @limit OFFSET @offset";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("text", $"%{text}%");

            IncludeModel includeCity = new IncludeModel();
            includeCity.As = "city";
            includeCity.ForeignKey = "city_id";
            includeCity.ThenInclude<State>(new IncludeModel{
                As = "state",
                ForeignKey = "state_id",
            });

            SelectOptions options = new SelectOptions();
            options.As = "aerodrome";
            options.Where = where;
            options.Params = queryParams;
            options.Include<City>(includeCity);

            IEnumerable<Aerodrome> aerodromes = await _dapperWrapper.QueryAsync<Aerodrome>(options);
            int total_records = aerodromes.Count();

            PaginationResult<Aerodrome> paginationResult = _utils.CreatePaginationResult<Aerodrome>(aerodromes.ToList(), filter, total_records);

            return paginationResult;
        }

        public async Task<Aerodrome> Update(Aerodrome aerodrome)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = aerodrome;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<Aerodrome>(options);
            return aerodrome;
        }
    }
}