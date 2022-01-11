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
    public class UserRoleService : BaseService, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IUserRoleRepository userRoleRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<UserRole> GetByName(string name)
        {
            UserRole userRole = await _userRoleRepository.GetByName(name);
            return userRole;
        }

        public async Task<IEnumerable<UserRole>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;

            PaginationResult<UserRole> paginationResult = await _userRoleRepository.Pagination(filter);
            List<UserRole> user_roles = paginationResult.data;

            return user_roles;
        }

        public async Task<PaginationResult<UserRole>> Pagination(PaginationFilter filter)
        {
            PaginationResult<UserRole> paginationResult = await _userRoleRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<UserRole> Save(UserRole userRole)
        {
            bool update = userRole.id != "";
            User user = _informer.GetValue<User>(UserTypes.User);

            if(update)
            {
                userRole.updated_by = user.id;
            }
            else
            {
                userRole.created_by = user.id;
                userRole = await _userRoleRepository.Create(userRole);
            }

            return userRole;
        }
    }
}