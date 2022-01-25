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

namespace clickfly.Repositories
{
    public class AircraftModelRepository : BaseRepository<AircraftModel>, IAircraftModelRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "aircraft_models as aircraft_model";
        private static string whereSql = "aircraft_model.excluded = false";
        private static string deleteSql = "UPDATE aircraft_models SET excluded = true WHERE id = @id";

        public AircraftModelRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            
        }

        public async Task<AircraftModel> Create(AircraftModel aircraftModel)
        {
            aircraftModel.id = Guid.NewGuid().ToString();
            aircraftModel.created_at = DateTime.Now;
            aircraftModel.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = aircraftModel;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<AircraftModel>(options);
            return aircraftModel;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public async Task<AircraftModel> GetById(string id)
        {
            SelectOptions options = new SelectOptions();
            options.As = "aircraft_model";
            options.Where = $"{whereSql} AND aircraft_model.id = @id";
            options.Params = new { id = id };

            options.Include<Manufacturer>(new IncludeModel{
                As = "manufacturer",
                ForeignKey = "manufacturer_id"
            });

            AircraftModel aircraftModel = await _dapperWrapper.QuerySingleAsync<AircraftModel>(options);
            return aircraftModel;
        }

        public async Task<PaginationResult<AircraftModel>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);

            SelectOptions options = new SelectOptions();
            options.As = "aircraft_model";
            options.Where = $"{whereSql} LIMIT @limit OFFSET @offset";
            options.Params = queryParams;

            IEnumerable<AircraftModel> aircraftModels = await _dapperWrapper.QueryAsync<AircraftModel>(options);
            int total_records = aircraftModels.Count();

            PaginationResult<AircraftModel> paginationResult = _utils.CreatePaginationResult<AircraftModel>(aircraftModels.ToList(), filter, total_records);

            return paginationResult;
        }

        public async Task<AircraftModel> Update(AircraftModel aircraftModel)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = aircraftModel;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<AircraftModel>(options);
            return aircraftModel;
        }
    }
}