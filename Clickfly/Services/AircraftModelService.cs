using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class AircraftModelService : BaseService, IAircraftModelService
    {
        private readonly IAircraftModelRepository _aircraftModelRepository;

        public AircraftModelService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IAircraftModelRepository aircraftModelRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
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

        public async Task Delete(string id)
        {
            await _aircraftModelRepository.Delete(id);
        }

        public async Task<AircraftModel> GetById(string id)
        {
            return(await _aircraftModelRepository.GetById(id));
        }

        public async Task<PaginationResult<AircraftModel>> Pagination(PaginationFilter filter)
        {
            PaginationResult<AircraftModel> paginationResult = await _aircraftModelRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<AircraftModel> Save(AircraftModel aircraftModel)
        {
            bool update = aircraftModel.id != "";

            if(update)
            {
                aircraftModel = await _aircraftModelRepository.Update(aircraftModel);
            }
            else
            {
                aircraftModel = await _aircraftModelRepository.Create(aircraftModel);
            }

            return aircraftModel;
        }
    }
}
