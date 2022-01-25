using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IStateService
    {
        Task<State> Save(State state);
        Task<PaginationResult<State>> Pagination(PaginationFilter filter);
        Task<IEnumerable<State>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<State> GetById(string id);
        Task<State> GetByPrefix(string prefix);
        Task Delete(string id);
    }
}