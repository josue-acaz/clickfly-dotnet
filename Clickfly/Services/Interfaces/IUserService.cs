using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface IUserService
    {
        Task<User> Save(User user);
        Task<PaginationResult<User>> Pagination(PaginationFilter filter);
        Task<User> GetById(string id);
        Task Delete(string id);
        Task<Authenticated> Authenticate(AuthenticateParams authenticateParams);
    }
}