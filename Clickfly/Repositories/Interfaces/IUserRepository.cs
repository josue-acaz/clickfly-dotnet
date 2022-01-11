using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<User> GetById(string id);
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task<User> Update(User user);
        Task Delete(string id);
        Task<PaginationResult<User>> Pagination(PaginationFilter filter);
    }
}
