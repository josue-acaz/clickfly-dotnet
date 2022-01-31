using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Exceptions;
using clickfly.Helpers;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class AerodromeService : BaseService, IAerodromeService
    {
        private readonly IAerodromeRepository _aerodromeRepository;
        private readonly ICityRepository _cityRepository;

        public AerodromeService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IAerodromeRepository aerodromeRepository, 
            ICityRepository cityRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _aerodromeRepository = aerodromeRepository;
            _cityRepository = cityRepository;
        }

        public async Task Delete(string id)
        {
            await _aerodromeRepository.Delete(id);
        }

        public async Task<Aerodrome> GetById(string id)
        {
            Aerodrome aerodrome = await _aerodromeRepository.GetById(id);
            return aerodrome;
        }

        public async Task<PaginationResult<Aerodrome>> Pagination(PaginationFilter filter)
        {
            PaginationResult<Aerodrome> paginationResult = await _aerodromeRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<IEnumerable<Aerodrome>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;

            PaginationResult<Aerodrome> paginationResult = await _aerodromeRepository.Pagination(filter);
            List<Aerodrome> aerodromes = paginationResult.data;

            return aerodromes;
        }

        public async Task<Aerodrome> Save(Aerodrome aerodrome)
        {
            bool update = aerodrome.id != "";

            if(update)
            {
                aerodrome = await _aerodromeRepository.Update(aerodrome);
            }
            else
            {
                aerodrome = await _aerodromeRepository.Create(aerodrome);
            }

            return aerodrome;
        }

        public async Task<Aerodrome> SaveExternal(Aerodrome aerodrome)
        {
            string cityName = aerodrome.city_name;
            string statePrefix = aerodrome.state_prefix;

            City city = await _cityRepository.GetByName(cityName, statePrefix);
            if(city == null)
            {
                throw new NotFoundException("Cidade não encontrada.");
            }

            aerodrome.city_id = city.id;
            aerodrome = await _aerodromeRepository.Create(aerodrome);

            return aerodrome;
        }
    }
}
