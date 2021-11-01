using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace clickfly.Repositories
{
    public class AircraftImageRepository : BaseRepository<AircraftImage>, IAircraftImageRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "aircraft_images as aircraft_image";
        private static string whereSql = "aircraft_image.excluded = false";
        private static string innerJoinFiles = "files as file on aircraft_image.id = file.resource_id";

        public AircraftImageRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils) : base(dBContext, dataContext, utils)
        {

        }

        public async Task<AircraftImage> Create(AircraftImage aircraftImage)
        {
            aircraftImage.id = Guid.NewGuid().ToString();

            await _dataContext.AircraftImages.AddAsync(aircraftImage);
            await _dataContext.SaveChangesAsync();

            return aircraftImage;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
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

        public Task<AircraftImage> Update(AircraftImage aircraftImage, string[] fields = null)
        {
            throw new NotImplementedException();
        }
    }
}