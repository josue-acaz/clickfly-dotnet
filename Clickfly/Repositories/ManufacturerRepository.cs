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
    public class ManufacturerRepository : BaseRepository<Manufacturer>, IManufacturerRepository
    {
        private static string fieldsSql = "*";
        private static string whereSql = "manufacturer.excluded = false";
        private static string deleteSql = "UPDATE manufacturers SET excluded = true WHERE id = @id";

        public ManufacturerRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Manufacturer> Create(Manufacturer manufacturer)
        {
            manufacturer.id = Guid.NewGuid().ToString();
            manufacturer.created_at = DateTime.Now;
            manufacturer.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = manufacturer;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Manufacturer>(options);
            return manufacturer;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public Task<Manufacturer> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<Manufacturer>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string air_taxi_id = filter.air_taxi_id;

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);

            SelectOptions options = new SelectOptions();
            options.As = "manufacturer";
            options.Where = $"{whereSql} LIMIT @limit OFFSET @offset";
            options.Params = queryParams;

            IEnumerable<Manufacturer> manufacturers = await _dapperWrapper.QueryAsync<Manufacturer>(options);
            int total_records = manufacturers.Count();

            PaginationResult<Manufacturer> paginationResult = _utils.CreatePaginationResult<Manufacturer>(manufacturers.ToList(), filter, total_records);

            return paginationResult;
        }

        public async Task<Manufacturer> Update(Manufacturer manufacturer)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = manufacturer;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<Manufacturer>(options);
            return manufacturer;
        }
    }
}