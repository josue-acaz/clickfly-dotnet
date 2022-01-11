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
    public class AircraftRepository : BaseRepository<Aircraft>, IAircraftRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "aircrafts as aircraft";
        private static string whereSql = "aircraft.excluded = false";
        private static string aircraftThumbnailSql = $"SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";
        private static string aircraftSeatingMapSql = $"SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'seating_map' LIMIT 1";

        public AircraftRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Aircraft> Create(Aircraft aircraft)
        {
            aircraft.id = Guid.NewGuid().ToString();

            await _dataContext.Aircrafts.AddAsync(aircraft);
            await _dataContext.SaveChangesAsync();

            return aircraft;
        }

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE aircrafts as aircraft set aircraft.excluded = true WHERE aircraft.id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<Aircraft> GetById(string id)
        {
            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            IncludeModel includeFlight = new IncludeModel();
            includeFlight.As = "flights";
            includeFlight.Where = "flights.excluded = false";
            includeFlight.ForeignKey = "aircraft_id";

            SelectOptions options = new SelectOptions();
            options.As = "aircraft";
            options.Where = $"{whereSql} AND aircraft.id = @id";
            options.Params = new { id = id };
            options.AddRawAttribute("thumbnail", aircraftThumbnailSql);
            options.AddRawAttribute("seating_map", aircraftSeatingMapSql);
            options.Include<Flight>(includeFlight);
            options.Include<AircraftModel>(includeAircraftModel);

            Aircraft aircraft = await _dapperWrapper.QuerySingleAsync<Aircraft>(options);

            return aircraft;
        }

        public async Task<string> GetThumbnail(GetThumbnailRequest thumbnailRequest)
        {
            string type = thumbnailRequest.type;
            string aircraftId = thumbnailRequest.aircraft_id;

            string querySql = $@"SELECT file.url as {type} FROM files as file WHERE file.excluded = false AND file.resource_id = @aircraft_id AND file.field_name = @type LIMIT 1 OFFSET 0";
        
            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("@type", type);
            queryParams.Add("@aircraft_id", aircraftId);
            
            string thumbnail = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<string>(querySql, queryParams);
            return thumbnail;
        }

        public async Task<PaginationResult<Aircraft>> Pagination(PaginationFilter filter)
        {
            string airTaxiId = filter.air_taxi_id;
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            List<Aircraft> aircrafts = await _dataContext.Aircrafts
                .Include(aircraft => aircraft.model)
                .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
                .Take(paginationFilter.page_size)
                .Where(aircraft => aircraft.air_taxi_id == airTaxiId && aircraft.excluded == false)
                .ToListAsync();
            
            int total_records = await _dataContext.Aircrafts.CountAsync();
            PaginationResult<Aircraft> paginationResult = _utils.CreatePaginationResult<Aircraft>(aircrafts, paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<Aircraft> Update(Aircraft aircraft)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = aircraft;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<Aircraft>(options);
            return aircraft;
        }
    }
}