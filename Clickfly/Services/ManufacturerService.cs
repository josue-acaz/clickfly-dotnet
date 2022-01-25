using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Helpers;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;
using clickfly.Exceptions;
using System.Collections.Generic;

namespace clickfly.Services
{
    public class ManufacturerService : BaseService, IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IOptions<AppSettings> appSettings, ISystemLogRepository systemLogRepository, IPermissionRepository permissionRepository, INotificator notificator, IInformer informer, IUtils utils, IManufacturerRepository manufacturerRepository) : base(appSettings, systemLogRepository, permissionRepository, notificator, informer, utils)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<IEnumerable<Manufacturer>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;

            PaginationResult<Manufacturer> paginationResult = await _manufacturerRepository.Pagination(filter);
            List<Manufacturer> manufacturers = paginationResult.data;

            return manufacturers;
        }

        public async Task Delete(string id)
        {
            await _manufacturerRepository.Delete(id);
        }

        public Task<Manufacturer> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<Manufacturer>> Pagination(PaginationFilter filter)
        {
            PaginationResult<Manufacturer> paginationResult = await _manufacturerRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<Manufacturer> Save(Manufacturer manufacturer)
        {
            bool update = manufacturer.id != "";

            if(update)
            {
                manufacturer = await _manufacturerRepository.Update(manufacturer);
            }
            else
            {
                manufacturer = await _manufacturerRepository.Create(manufacturer);
            }

            return manufacturer;
        }
    }
}
