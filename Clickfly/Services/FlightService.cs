using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using clickfly.Helpers;
using Microsoft.Extensions.Options;

namespace clickfly.Services
{
    public class FlightService : BaseService, IFlightService
    {
        private readonly IFlightRepository _flightRepository;

        public FlightService(
            IOptions<AppSettings> appSettings, 
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IFlightRepository flightRepository
        ) : base(
            appSettings,
            notificator,
            informer,
            utils
        )
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
            User user = _informer.GetValue<User>(UserTypes.User);
            filter.air_taxi_id = user.air_taxi_id;

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
                User user = _informer.GetValue<User>(UserTypes.User);
                flight.air_taxi_id = user.air_taxi_id;

                flight = await _flightRepository.Create(flight);
            }

            return flight;
        }
    }
}