using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface IAerodromeService
    {
        Task<Aerodrome> Save(Aerodrome aerodrome);
        Task<Aerodrome> GetById(string id);
        Task Delete(string id);
        Task<IEnumerable<Aerodrome>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<PaginationResult<Aerodrome>> Pagination(PaginationFilter filter);
    }
}
