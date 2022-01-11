using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IPermissionService
    {
        Task<Permission> Save(Permission permission);
        Task Delete(string id);
        Task<PaginationResult<Permission>> Pagination(PaginationFilter filter);
    }
}