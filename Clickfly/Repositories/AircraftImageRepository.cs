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
    public class AircraftImageRepository : BaseRepository<AircraftImage>, IAircraftImageRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "aircraft_images as aircraft_image";
        private static string whereSql = "aircraft_image.excluded = false";
        private static string innerJoinFiles = "files as file on aircraft_image.id = file.resource_id";
        private static string deleteSql = "UPDATE aircraft_images SET excluded = true WHERE id = @id";

        public AircraftImageRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<AircraftImage> Create(AircraftImage aircraftImage)
        {
            aircraftImage.id = Guid.NewGuid().ToString();
            aircraftImage.created_at = DateTime.Now;
            aircraftImage.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = aircraftImage;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<AircraftImage>(options);
            return aircraftImage;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public Task<AircraftImage> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<AircraftImage>> Pagination(AircraftImagePaginationFilter filter)
        {
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            string aircraftId = filter.aircraft_id;
            int limit = paginationFilter.page_size;
            int offset = (paginationFilter.page_number - 1) * paginationFilter.page_size;

            string querySql = $@"SELECT 
                aircraft_image.id, 
                aircraft_image.view, 
                aircraft_image.aircraft_id, 
                file.url 
                FROM {fromSql} INNER JOIN {innerJoinFiles} WHERE {whereSql} AND aircraft_image.aircraft_id = @aircraft_id ORDER BY aircraft_image.view DESC LIMIT @limit OFFSET @offset";
            
            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("aircraft_id", aircraftId);

            IEnumerable<AircraftImage> aircraftImages = await _dBContext.GetConnection().QueryAsync<AircraftImage>(querySql, queryParams, _dBContext.GetTransaction());
            
            int total_records = await _dataContext.AircraftImages.CountAsync();
            PaginationResult<AircraftImage> paginationResult = _utils.CreatePaginationResult<AircraftImage>(aircraftImages.ToList<AircraftImage>(), paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<AircraftImage> Update(AircraftImage aircraftImage)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = aircraftImage;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<AircraftImage>(options);
            return aircraftImage;
        }
    }
}