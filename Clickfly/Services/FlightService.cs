using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Helpers;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;
using clickfly.Exceptions;

namespace clickfly.Services
{
    public class FlightService : BaseService, IFlightService
    {
        private readonly IFlightRepository _flightRepository;

        public FlightService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IFlightRepository flightRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _flightRepository = flightRepository;
        }

        public async Task Delete(string id)
        {
            User user = _informer.GetValue<User>(UserTypes.User);

            /*
            bool hasPermission = await _permissionRepository.HasPermission(user.id, Resources.Flights, Actions.Delete);
            if(!hasPermission)
            {
                throw new UnauthorizedException("Você não tem permissão para excluir.");
            }
            */

            await _flightRepository.Delete(id);
        }

        public async Task<Flight> GetById(string id)
        {
            User user = _informer.GetValue<User>(UserTypes.User);

            bool hasPermission = await _permissionRepository.HasPermission(user.id, Resources.Flights, Actions.Read);
            if(!hasPermission)
            {
                throw new UnauthorizedException("Você não tem permissão para ver.");
            }

            Flight flight = await _flightRepository.GetById(id);
            return flight;
        }

        public async Task<PaginationResult<Flight>> Pagination(PaginationFilter filter)
        {
            User user = _informer.GetValue<User>(UserTypes.User);

            bool hasPermission = await _permissionRepository.HasPermission(user.id, Resources.Flights, Actions.Read);
            if(!hasPermission)
            {
                throw new UnauthorizedException("Você não tem permissão para ver.");
            }
            
            filter.air_taxi_id = user.air_taxi_id;
            PaginationResult<Flight> paginationResult = await _flightRepository.Pagination(filter);
            
            return paginationResult;
        }

        public async Task<Flight> Save(Flight flight)
        {
            bool update = flight.id != "";
            User user = _informer.GetValue<User>(UserTypes.User);

            if(update)
            {
                bool hasPermission = await _permissionRepository.HasPermission(user.id, Resources.Flights, Actions.Update);
                if(!hasPermission)
                {
                    throw new UnauthorizedException("Você não tem permissão para editar.");
                }

                flight.updated_by = user.id;
                await _flightRepository.Update(flight);
            }
            else
            {
                bool hasPermission = await _permissionRepository.HasPermission(user.id, Resources.Flights, Actions.Create);
                if(!hasPermission)
                {
                    throw new UnauthorizedException("Você não tem permissão para criar.");
                }

                flight.created_by = user.id;
                flight.air_taxi_id = user.air_taxi_id;
                flight = await _flightRepository.Create(flight);
            }
            
            return flight;
        }
    }
}
