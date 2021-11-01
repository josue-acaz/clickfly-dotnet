using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IStateRepository
    {
        Task<State> Create(State state);
        Task<State> GetById(string id);
        Task<State> GetByPrefix(string prefix);
        Task Update(State state);
        Task Delete(string id);
        Task<PaginationResult<State>> Pagination(PaginationFilter filter);
    }
}