using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class AircraftRepository : BaseRepository<Aircraft>, IAircraftRepository
    {
        protected readonly IUploadService _uploadService;
        private static string fieldsSql = "*";
        private static string fromSql = "aircrafts as aircraft";
        private static string whereSql = "aircraft.excluded = false";
        //private static string aircraftThumbnailSql = $"SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'thumbnail' LIMIT 1";
        //private static string aircraftSeatingMapSql = $"SELECT url FROM files WHERE resource_id = aircraft.id AND resource = 'aircrafts' AND field_name = 'seating_map' LIMIT 1";

        public AircraftRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils, IUploadService uploadService) : base(dBContext, dataContext, dapperWrapper, utils)
        {
            _uploadService = uploadService;
        }

        public async Task<Aircraft> Create(Aircraft aircraft)
        {
            aircraft.id = Guid.NewGuid().ToString();
            aircraft.created_at = DateTime.Now;
            aircraft.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = aircraft;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Aircraft>(options);
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
            options.Include<Flight>(includeFlight);
            options.Include<AircraftModel>(includeAircraftModel);

            Aircraft aircraft = await _dapperWrapper.QuerySingleAsync<Aircraft>(options);

            File thumbnailFile = await _dapperWrapper.QuerySingleAsync<File>(new SelectOptions{
                As = "file",
                Where = $"file.excluded = false AND file.resource_id = @aircraft_id AND file.field_name = 'thumbnail'",
                Params = new { aircraft_id = aircraft.id },
            });

            File seatingMapFile = await _dapperWrapper.QuerySingleAsync<File>(new SelectOptions{
                As = "file",
                Where = $"file.excluded = false AND file.resource_id = @aircraft_id AND file.field_name = 'seating_map'",
                Params = new { aircraft_id = aircraft.id },
            });

            if(thumbnailFile != null)
            {
                aircraft.thumbnail = _uploadService.GetPreSignedUrl(thumbnailFile.key);
            }
            if(seatingMapFile != null)
            {
                aircraft.seating_map = _uploadService.GetPreSignedUrl(seatingMapFile.key);
            }

            return aircraft;
        }

        public async Task<string> GetThumbnail(GetThumbnailRequest thumbnailRequest)
        {
            string type = thumbnailRequest.type;
            string aircraft_id = thumbnailRequest.aircraft_id;

            File thumbnailFile = await _dapperWrapper.QuerySingleAsync<File>(new SelectOptions{
                As = "file",
                Where = $"file.excluded = false AND file.resource_id = @aircraft_id AND file.field_name = @type LIMIT 1",
                Params = new { type = type, aircraft_id = aircraft_id },
            });
            
            return _uploadService.GetPreSignedUrl(thumbnailFile.key);
        }

        public async Task<PaginationResult<Aircraft>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string air_taxi_id = filter.air_taxi_id;
            string text = filter.text;

            string where = $"{whereSql} AND aircraft.prefix ILIKE @text LIMIT @limit OFFSET @offset";

            PaginationFilter paginationFilter = new PaginationFilter(filter.page_number, filter.page_size);

            var queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("text", $"%{text}%");
            queryParams.Add("air_taxi_id", air_taxi_id);

            SelectOptions options = new SelectOptions();
            options.As = "aircraft";
            options.Where = where;
            options.MainWhere = $"model.name ILIKE @text";
            options.Params = queryParams;

            options.Include<AircraftModel>(new IncludeModel{
                As = "model",
                ForeignKey = "aircraft_model_id",
            });

            IEnumerable<Aircraft> aircrafts = await _dapperWrapper.QueryAsync<Aircraft>(options);
            int total_records = aircrafts.Count();

            PaginationResult<Aircraft> paginationResult = _utils.CreatePaginationResult<Aircraft>(aircrafts.ToList(), paginationFilter, total_records);

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