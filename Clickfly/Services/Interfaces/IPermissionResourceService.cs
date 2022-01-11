using System;
using System.Threading.Tasks;
using clickfly.Models;
using System.Collections.Generic;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IPermissionResourceService
    {
        Task<PermissionResource> Save(PermissionResource permissionResource);
        Task<PermissionResource> GetByName(string id);
        Task Delete(string id);
        Task<IEnumerable<PermissionResource>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<PaginationResult<PermissionResource>> Pagination(PaginationFilter filter);
    }
}