using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using System.Collections.Generic;
using clickfly.ViewModels;
using System.Linq;

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
            User authUser = _informer.GetValue<User>(UserTypes.User);
            string role = authUser.role;

            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;

            // Excluir perfis de mesmo nível e acima
            string[] roles = new string[]{
                UserRoles.GENERAL_ADMINISTRATOR,
                UserRoles.ADMINISTRATOR,
                UserRoles.MANAGER,
                UserRoles.EMPLOYEE,
            };

            int index = Array.FindIndex(roles, x => x == role);
            string[] excludeRoles = roles.Take(index + 1).ToArray();

            for (int i = 0; i < excludeRoles.Length; i++)
            {
                filter.exclude.Add(new ExcludeFilterAttribute{
                    name = "name",
                    value = excludeRoles[i]
                });
            }

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

        public async Task Delete(string id)
        {
            await _userRoleRepository.Delete(id);
        }
    }
}