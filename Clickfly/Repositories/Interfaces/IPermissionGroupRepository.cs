using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IPermissionGroupRepository
    {
        Task<PermissionGroup> Create(PermissionGroup permissionGroup);
        Task<PermissionGroup> GetById(string id);
        Task<PermissionGroup> GetByUserId(string user_id);
        Task<PermissionGroup> Update(PermissionGroup permissionGroup);
        Task Delete(string id);
        Task<PaginationResult<PermissionGroup>> Pagination(PaginationFilter filter);
    }
}
