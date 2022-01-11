using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IUserRolePermissionRepository
    {
        Task<UserRolePermission> Create(UserRolePermission userRolePermission);
        Task<UserRolePermission> GetById(string id);
        Task<PaginationResult<UserRolePermission>> Pagination(PaginationFilter filter);
    }
}