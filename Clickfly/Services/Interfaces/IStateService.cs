using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface IStateService
    {
        Task<State> Save(State state);
        Task<PaginationResult<State>> Pagination(PaginationFilter filter);
        Task<State> GetById(string id);
        Task<State> GetByPrefix(string prefix);
        Task Delete(string id);
    }
}