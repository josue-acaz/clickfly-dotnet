using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;
using Microsoft.Extensions.Options;

namespace clickfly.Repositories
{
    public class AirTaxiRepository : BaseRepository<AirTaxi>, IAirTaxiRepository
    {
        private readonly AppSettings _appSettings;
        private static string fieldsSql = "*";
        private static string fromSql = "air_taxis as air_taxi";
        private static string whereSql = "air_taxi.excluded = false";

        public AirTaxiRepository(IOptions<AppSettings> appSettings, IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            _appSettings = appSettings.Value;
        }
        
        public async Task<AirTaxi> Create(AirTaxi airTaxi)
        {
            airTaxi.id = Guid.NewGuid().ToString();
            airTaxi.created_at = DateTime.Now;
            airTaxi.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = airTaxi;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<AirTaxi>(options);
            return airTaxi;
        }

        public Task Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AirTaxi> GetById(string id)
        {
            SelectOptions options = new SelectOptions();
            options.As = "air_taxi";
            options.Where = $"{whereSql} AND id = @id";
            options.Params = new { id = id };

            AirTaxi airTaxi = await _dapperWrapper.QuerySingleAsync<AirTaxi>(options);
            return airTaxi;
        }

        public async Task<PaginationResult<AirTaxi>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;

            var queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);

            IncludeModel includeAccessToken = new IncludeModel();
            includeAccessToken.As = "access_token";
            includeAccessToken.ForeignKey = "resource_id";
            
            SelectOptions options = new SelectOptions();
            options.As = "air_taxi";
            options.Where = whereSql;
            options.Params = queryParams;
            options.Include<AccessToken>(includeAccessToken);
            
            options.AddRawAttribute("dashboard_url", $"CONCAT('{_appSettings.DashboardUrl}', '/login?token=', access_token.token)");

            int total_records = _dapperWrapper.Count<AirTaxi>(new CountOptions {
                As = "air_taxi",
                Where = whereSql,
                Params = queryParams,
            });

            IEnumerable<AirTaxi> airTaxis = await _dapperWrapper.QueryAsync<AirTaxi>(options);
            PaginationResult<AirTaxi> paginationResult = _utils.CreatePaginationResult<AirTaxi>(airTaxis.ToList(), filter, total_records);

            return paginationResult;
        }

        public async Task<AirTaxi> Update(AirTaxi airTaxi)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = airTaxi;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<AirTaxi>(options);
            return airTaxi;
        }
    }
}