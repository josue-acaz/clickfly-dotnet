using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;

namespace clickfly.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Flight> GetById(string id)
        {
            Flight flight = await _flightRepository.GetById(id);
            return flight;
        }

        public async Task<PaginationResult<Flight>> Pagination(PaginationFilter filter)
        {
            PaginationResult<Flight> paginationResult = await _flightRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<Flight> Save(Flight flight)
        {
            bool update = flight.id != "";

            if(update)
            {
                
            }
            else
            {
                flight = await _flightRepository.Create(flight);
            }

            return flight;
        }
    }
}