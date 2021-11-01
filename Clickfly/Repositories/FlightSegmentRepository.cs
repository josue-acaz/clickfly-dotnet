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
    public class FlightSegmentRepository : BaseRepository<FlightSegment>, IFlightSegmentRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "flight_segments as flight_segment";
        private static string whereSql = "flight_segment.excluded = false";
        protected string[] defaultFields = new string[10];

        public FlightSegmentRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils) : base(dBContext, dataContext, utils)
        {
            defaultFields[0] = "number";
            defaultFields[1] = "departure_datetime";
            defaultFields[2] = "arrival_datetime";
            defaultFields[3] = "price_per_seat";
            defaultFields[4] = "total_seats";
            defaultFields[5] = "type";
            defaultFields[6] = "flight_id";
            defaultFields[7] = "aircraft_id";
            defaultFields[8] = "origin_aerodrome_id";
            defaultFields[9] = "destination_aerodrome_id";
        }

        public async Task<FlightSegment> Create(FlightSegment flightSegment)
        {
            string id = Guid.NewGuid().ToString();
            flightSegment.id = id;

            await _dataContext.FlightSegments.AddAsync(flightSegment);
            await _dataContext.SaveChangesAsync();

            return flightSegment;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<FlightSegment> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND flight_segment.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            FlightSegment flightSegment = await _dataContext.FlightSegments
            .FromSqlRaw(querySql, param)
            .FirstOrDefaultAsync();
            
            return flightSegment;
        }

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            string flightId = filter.flight_id;

            List<FlightSegment> flightSegments = await _dataContext.FlightSegments
                .Include(flightSegment => flightSegment.aircraft)
                .ThenInclude(aircraft => aircraft.model)
                .Include(flightSegment => flightSegment.origin_aerodrome)
                .Include(flightSegment => flightSegment.destination_aerodrome)
                .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
                .Take(paginationFilter.page_size)
                .Where(flightSegment => flightSegment.excluded == false && flightSegment.flight_id == flightId)
                .ToListAsync();
            
            int total_records = await _dataContext.Flights.CountAsync();
            PaginationResult<FlightSegment> paginationResult = _utils.CreatePaginationResult<FlightSegment>(flightSegments, paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<FlightSegment> Update(FlightSegment flightSegment, string[] fields = null)
        {
            if(fields == null)
            {
                fields = defaultFields;
            }

            GetFieldsSqlParams fieldsSqlParams = new GetFieldsSqlParams();
            fieldsSqlParams.action = "UPDATE";
            fieldsSqlParams.fields = fields;

            string fieldsToUpdate = _utils.GetFieldsSql(fieldsSqlParams);
            string querySql = $"UPDATE flight_segments SET {fieldsToUpdate} WHERE id = @id";

            await _dBContext.GetConnection().ExecuteAsync(querySql, flightSegment, _dBContext.GetTransaction());

            return flightSegment;
        }
    }
}
