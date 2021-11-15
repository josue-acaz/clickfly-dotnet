using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Npgsql;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace clickfly.Repositories
{
    public class FlightRepository : BaseRepository<Flight>, IFlightRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "flights as flight";
        private static string whereSql = "flight.excluded = false";

        public FlightRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {

        }

        public async Task<Flight> Create(Flight flight)
        {
            string id = Guid.NewGuid().ToString();
            flight.id = id;

            await _dataContext.Flights.AddAsync(flight);
            await _dataContext.SaveChangesAsync();

            return flight;
        }

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE flights as flight set flight.excluded = true WHERE flight.id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<Flight> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND flight.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            Flight flight = await _dataContext.Flights
            .FromSqlRaw(querySql, param)
            .Include(flight => flight.aircraft)
            .ThenInclude(aircraft => aircraft.model)
            .Include(flight => flight.segments)
            .ThenInclude(flightSegment => flightSegment.origin_aerodrome)
            .Include(flight => flight.segments)
            .ThenInclude(flightSegment => flightSegment.destination_aerodrome)
            .FirstOrDefaultAsync();

            return flight;
        }

        public async Task<FlightSegment> GetLastSegment(string flightId)
        {
            string querySql = $"SELECT * FROM flight_segments AS flight_segment WHERE flight_segment.excluded = false AND flight_segment.flight_id = @flight_id ORDER BY flight_segment.number DESC LIMIT 1 OFFSET 0";
            NpgsqlParameter param = new NpgsqlParameter("flight_id", flightId);
            
            FlightSegment flightSegment = await _dataContext.FlightSegments.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return flightSegment;
        }

        public async Task<PaginationResult<Flight>> Pagination(PaginationFilter filter)
        {
            string airTaxiId = filter.air_taxi_id;
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            List<Flight> flights = await _dataContext.Flights
                .Include(flight => flight.aircraft)
                .ThenInclude(aircraft => aircraft.model)
                .Include(flight => flight.segments)
                .ThenInclude(segment => segment.origin_aerodrome)
                .ThenInclude(origin_aerodrome => origin_aerodrome.city)
                .ThenInclude(city => city.state)
                .Include(flight => flight.segments)
                .ThenInclude(segment => segment.destination_aerodrome)
                .ThenInclude(destination_aerodrome => destination_aerodrome.city)
                .ThenInclude(city => city.state)
                .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
                .Take(paginationFilter.page_size)
                .Where(flight => flight.air_taxi_id == airTaxiId && flight.excluded == false)
                .ToListAsync();
            
            int total_records = await _dataContext.Flights.CountAsync();
            PaginationResult<Flight> paginationResult = _utils.CreatePaginationResult<Flight>(flights, paginationFilter, total_records);

            return paginationResult;
        }

        public async Task Update(Flight flight)
        {
            throw new NotImplementedException();
        }
    }
}