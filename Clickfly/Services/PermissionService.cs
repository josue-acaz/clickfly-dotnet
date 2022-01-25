using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using clickfly.Exceptions;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class PermissionService : BaseService, IPermissionService
    {
        private readonly IPermissionResourceRepository _permissionResourceRepository;
        private readonly IPermissionGroupRepository _permissionGroupRepository;

        public PermissionService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            IPermissionGroupRepository permissionGroupRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _permissionGroupRepository = permissionGroupRepository;
        }

        public async Task Delete(string id)
        {
            await _permissionRepository.Delete(id);
        }

        public async Task<PaginationResult<Permission>> Pagination(PaginationFilter filter)
        {
            PaginationResult<Permission> paginationResult = await _permissionRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<Permission> Save(Permission permission)
        {
            bool update = permission.id != "";
            User user = _informer.GetValue<User>(UserTypes.User);

            if(update)
            {
                permission.updated_by = user.id;
                await _permissionRepository.Update(permission);
            }
            else
            { 
                PermissionResource permissionResource = await _permissionResourceRepository.GetById(permission.permission_resource_id);
                Permission existPermission = await _permissionRepository.Exists(user.id, permissionResource._table);
                
                if(existPermission != null)
                {
                    throw new ConflictException("Permissão já concedida.");
                }

                PermissionGroup permissionGroup = await _permissionGroupRepository.GetByUserId(user.id);

                permission.created_by = user.id;
                permission.permission_group_id = permissionGroup.id;
                permission = await _permissionRepository.Create(permission);
            }

            return permission;
        }
    }
}