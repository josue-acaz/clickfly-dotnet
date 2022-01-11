using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IPermissionResourceRepository
    {
        Task<PermissionResource> Create(PermissionResource permissionResource);
        Task<PermissionResource> GetByName(string name);
        Task<PermissionResource> GetById(string id);
        Task Update(PermissionResource permissionResource);
        Task Delete(string id);
        Task<PaginationResult<PermissionResource>> Pagination(PaginationFilter filter);
    }
}