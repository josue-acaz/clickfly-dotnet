using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IStateRepository
    {
        Task<State> Create(State state);
        Task<State> GetById(string id);
        Task<State> GetByPrefix(string prefix);
        Task<State> Update(State state);
        Task Delete(string id);
        Task<PaginationResult<State>> Pagination(PaginationFilter filter);
    }
}