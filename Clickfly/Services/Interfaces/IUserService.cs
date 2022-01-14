using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IUserService
    {
        Task<User> Save(User user);
        Task<User> UpdateRole(UpdateRole updateRole);
        Task ChangePassword(ChangePassword changePassword);
        Task<PaginationResult<User>> Pagination(PaginationFilter filter);
        Task<User> GetById(string id);
        Task Delete(string id);
        Task<Authenticated> Authenticate(AuthenticateParams authenticateParams);
    }
}