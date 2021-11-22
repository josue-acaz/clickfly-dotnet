using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;

namespace clickfly.Services
{
    public class AirTaxiBaseService : BaseService, IAirTaxiBaseService
    {
        private readonly IAirTaxiBaseRepository _airTaxiBaseRepository;

        public AirTaxiBaseService(
            IOptions<AppSettings> appSettings, 
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IAirTaxiBaseRepository airTaxiBaseRepository
        ) : base(
            appSettings,
            notificator,
            informer,
            utils
        )
        {
            _airTaxiBaseRepository = airTaxiBaseRepository;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AirTaxiBase> GetById(string id)
        {
            AirTaxiBase airTaxiBase = await _airTaxiBaseRepository.GetById(id);
            return airTaxiBase;
        }

        public async Task<PaginationResult<AirTaxiBase>> Pagination(PaginationFilter filter)
        {
            User user = _informer.GetValue<User>(UserTypes.User);
            filter.air_taxi_id = user.air_taxi_id;

            PaginationResult<AirTaxiBase> paginationResult = await _airTaxiBaseRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<AirTaxiBase> Save(AirTaxiBase airTaxiBase)
        {
            bool update = airTaxiBase.id != "";

            if(update)
            {
                airTaxiBase = await _airTaxiBaseRepository.Update(airTaxiBase);
            }
            else
            {
                User user = _informer.GetValue<User>(UserTypes.User);
                airTaxiBase.air_taxi_id = user.air_taxi_id;

                airTaxiBase = await _airTaxiBaseRepository.Create(airTaxiBase);
            }

            return airTaxiBase;
        }
    }
}
