using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using clickfly.Exceptions;
using System.Collections.Generic;

namespace clickfly.Services
{
    public class AerodromeService : IAerodromeService
    {
        private readonly IAerodromeRepository _aerodromeRepository;
        private readonly ICityRepository _cityRepository;

        public AerodromeService(IAerodromeRepository aerodromeRepository, ICityRepository cityRepository)
        {
            _aerodromeRepository = aerodromeRepository;
            _cityRepository = cityRepository;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
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

            PaginationResult<Aerodrome> paginationResult = await _aerodromeRepository.Pagination(filter);
            List<Aerodrome> aerodromes = paginationResult.data;

            return aerodromes;
        }

        public async Task<Aerodrome> Save(Aerodrome aerodrome)
        {
            bool update = aerodrome.id != "";

            if(update)
            {
                
            }
            else
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
            }

            return aerodrome;
        }
    }
}
