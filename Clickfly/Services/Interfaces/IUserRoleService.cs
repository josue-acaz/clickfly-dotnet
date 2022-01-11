using System;
using System.Threading.Tasks;
using clickfly.Models;
using System.Collections.Generic;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IUserRoleService
    {
        Task<UserRole> Save(UserRole userRole);
        Task<UserRole> GetByName(string name);
        Task<IEnumerable<UserRole>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<PaginationResult<UserRole>> Pagination(PaginationFilter filter);
    }
}