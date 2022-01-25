using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Helpers;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;
using System.Collections.Generic;

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

        public async Task Delete(string id)
        {
            await _cityRepository.Delete(id);
        }

        public Task<City> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<City>> Pagination(PaginationFilter filter)
        {
            PaginationResult<City> paginationResult = await _cityRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<IEnumerable<City>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;

            PaginationResult<City> paginationResult = await _cityRepository.Pagination(filter);
            List<City> cities = paginationResult.data;

            return cities;
        }

        public async Task<City> Save(City city)
        {
            bool update = city.id != "";

            if(update)
            {
                city = await _cityRepository.Update(city);
            }
            else
            {
                city = await _cityRepository.Create(city);
            }

            return city;
        }

        public async Task<City> SaveExternal(City city)
        {
            int gmt = city.gmt;
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(city));
            Timezone timezone = await _timezoneRepository.GetByGmt(gmt);

            string state_prefix = city.state_prefix;
            State state = await _stateRepository.GetByPrefix(state_prefix);

            city.state_id = state.id;
            city.timezone_id = timezone.id;
            city = await _cityRepository.Create(city);

            return city;
        }
    }
}
