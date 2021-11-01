using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<User> GetById(string id);
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task Update(User user);
        Task Delete(string id);
        Task<PaginationResult<User>> Pagination(PaginationFilter filter);
    }
}
