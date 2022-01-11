using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Helpers;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class CityService : BaseService, ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ITimezoneRepository _timezoneRepository;

        private readonly IStateRepository _stateRepository;

        public CityService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            ICityRepository cityRepository, 
            ITimezoneRepository timezoneRepository, 
            IStateRepository stateRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _cityRepository = cityRepository;
            _timezoneRepository = timezoneRepository;
            _stateRepository = stateRepository;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<City> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<City>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<City> Save(City city)
        {
            bool update = city.id != "";

            if(update)
            {
                
            }
            else
            {
                int gmt = city.gmt;
                Timezone timezone = await _timezoneRepository.GetByGmt(gmt);

                string state_prefix = city.state_prefix;
                State state = await _stateRepository.GetByPrefix(state_prefix);

                city.state_id = state.id;
                city.timezone_id = timezone.id;
                city = await _cityRepository.Create(city);
            }

            return city;
        }
    }
}
