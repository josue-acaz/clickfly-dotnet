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
    public class AircraftRepository : BaseRepository<Aircraft>, IAircraftRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "aircrafts as aircraft";
        private static string whereSql = "aircraft.excluded = false";
        private static string aircraftThumbnailSql = $"SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";
        private static string aircraftSeatingMapSql = $"SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'seating_map' LIMIT 1";
        protected string[] defaultFields = new string[17];

        public AircraftRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {
            defaultFields[0] = "prefix";
            defaultFields[1] = "year";
            defaultFields[2] = "crew";
            defaultFields[3] = "passengers";
            defaultFields[4] = "empty_weight";
            defaultFields[5] = "autonomy";
            defaultFields[6] = "maximum_takeoff_weight";
            defaultFields[7] = "maximum_speed";
            defaultFields[8] = "cruising_speed";
            defaultFields[9] = "range";
            defaultFields[10] = "fixed_price";
            defaultFields[11] = "fixed_price_radius";
            defaultFields[12] = "price_per_km";
            defaultFields[13] = "description";
            defaultFields[14] = "pressurized";
            defaultFields[15] = "aircraft_model_id";
            defaultFields[16] = "updated_at";
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
            /*
            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            QueryOptions queryOptions = new QueryOptions();
            queryOptions.Where = $"{whereSql} AND aircraft.id = @id";
            queryOptions.Params = new { id = id };
            queryOptions.AddRawAttribute("thumbnail", aircraftThumbnailSql);
            queryOptions.AddRawAttribute("seating_map", aircraftSeatingMapSql);
            queryOptions.Include<AircraftModel>(includeAircraftModel);

            Aircraft aircraft = await _dBAccess.QuerySingleAsync<Aircraft>(queryOptions);
            */

            IncludeModel includeAircraftModel = new IncludeModel();
            includeAircraftModel.As = "model";
            includeAircraftModel.ForeignKey = "aircraft_model_id";

            IncludeModel includeFlight = new IncludeModel();
            includeFlight.As = "flights";
            includeFlight.Where = "flights.excluded = false";
            includeFlight.ForeignKey = "aircraft_id";

            QueryOptions queryOptions = new QueryOptions();
            queryOptions.As = "aircraft";
            queryOptions.Where = $"{whereSql} AND aircraft.id = @id";
            queryOptions.Params = new { id = id };
            queryOptions.Include<Flight>(includeFlight);
            queryOptions.Include<AircraftModel>(includeAircraftModel);

            Aircraft aircraft = await _dBAccess.QuerySingleAsync<Aircraft>(queryOptions);

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

        public async Task<Aircraft> Update(Aircraft aircraft, string[] fields = null)
        {
            if(fields == null)
            {
                fields = defaultFields;
            }

            GetFieldsSqlParams fieldsSqlParams = new GetFieldsSqlParams();
            fieldsSqlParams.action = "UPDATE";
            fieldsSqlParams.fields = fields;

            string fieldsToUpdate = _utils.GetFieldsSql(fieldsSqlParams);
            string querySql = $"UPDATE aircrafts SET {fieldsToUpdate} WHERE id = @id";

            await _dBContext.GetConnection().ExecuteAsync(querySql, aircraft, _dBContext.GetTransaction());

            return aircraft;
        }
    }
}