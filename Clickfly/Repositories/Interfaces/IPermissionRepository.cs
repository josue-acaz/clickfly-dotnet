using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IPermissionRepository
    {
        Task<Permission> Create(Permission permission);
        Task<Permission> GetById(string id);
        Task<Permission> Update(Permission permission);
        Task Delete(string id);
        Task<Permission> Exists(string userId, string table);
        Task<bool> HasPermission(string userId, string table, string action);
        Task<PaginationResult<Permission>> Pagination(PaginationFilter filter);
    }
}
