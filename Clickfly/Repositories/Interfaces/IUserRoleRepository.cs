using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole> Create(UserRole userRole);
        Task<UserRole> GetByName(string name);
        Task<UserRole> GetByUserId(string userId);
        Task Delete(string id);
        Task<PaginationResult<UserRole>> Pagination(PaginationFilter filter);
    }
}