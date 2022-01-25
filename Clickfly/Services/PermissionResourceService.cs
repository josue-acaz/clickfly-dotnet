using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using System.Collections.Generic;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class PermissionResourceService : BaseService, IPermissionResourceService
    {
        private readonly IPermissionResourceRepository _permissionResourceRepository;

        public PermissionResourceService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IPermissionResourceRepository permissionResourceRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _permissionResourceRepository = permissionResourceRepository;
        }

        public async Task Delete(string id)
        {
            await _permissionResourceRepository.Delete(id);
        }

        public Task<PermissionResource> GetByName(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<PermissionResource>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PermissionResource>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;

            PaginationResult<PermissionResource> paginationResult = await _permissionResourceRepository.Pagination(filter);
            List<PermissionResource> permission_resources = paginationResult.data;

            return permission_resources;
        }

        public async Task<PermissionResource> Save(PermissionResource permissionResource)
        {
            bool update = permissionResource.id != "";
            User user = _informer.GetValue<User>(UserTypes.User);

            if(update)
            {
                permissionResource.updated_by = user.id;
                await _permissionResourceRepository.Update(permissionResource);
            }
            else
            {
                permissionResource.created_by = user.id;
                permissionResource = await _permissionResourceRepository.Create(permissionResource);
            }

            permissionResource = await _permissionResourceRepository.GetById(permissionResource.id);

            return permissionResource;
        }
    }
}