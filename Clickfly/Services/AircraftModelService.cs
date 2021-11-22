using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using clickfly.Helpers;

namespace clickfly.Services
{
    public class AircraftModelService : BaseService, IAircraftModelService
    {
        private readonly IAircraftModelRepository _aircraftModelRepository;

        public AircraftModelService(
            IOptions<AppSettings> appSettings, 
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IAircraftModelRepository aircraftModelRepository
        ) : base(
            appSettings,
            notificator,
            informer,
            utils
        )
        {
            _aircraftModelRepository = aircraftModelRepository;
        }

        public async Task<IEnumerable<AircraftModel>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";

            PaginationResult<AircraftModel> paginationResult = await _aircraftModelRepository.Pagination(filter);
            List<AircraftModel> aircraftModels = paginationResult.data;

            return aircraftModels;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AircraftModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<AircraftModel>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<AircraftModel> Save(AircraftModel aircraftModel)
        {
            bool update = aircraftModel.id != "";

            if(update)
            {
                
            }
            else
            {
                aircraftModel = await _aircraftModelRepository.Create(aircraftModel);
            }

            return aircraftModel;
        }
    }
}
