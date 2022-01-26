using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IDoubleCheckService
    {
        Task<DoubleCheck> Save(DoubleCheck doubleCheck);
        Task<DoubleCheck> SaveExternal(DoubleCheck doubleCheck);
        Task<PaginationResult<DoubleCheck>> Pagination(PaginationFilter filter);
        Task<IEnumerable<DoubleCheck>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<DoubleCheck> GetById(string id);
        Task Delete(string id);
    }
}
