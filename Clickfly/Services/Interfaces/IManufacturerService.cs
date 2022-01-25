using System;
using System.Threading.Tasks;
using clickfly.Models;
using System.Collections.Generic;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IManufacturerService
    {
        Task<Manufacturer> Save(Manufacturer aircraftModel);
        Task<PaginationResult<Manufacturer>> Pagination(PaginationFilter filter);
        Task<IEnumerable<Manufacturer>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<Manufacturer> GetById(string id);
        Task Delete(string id);
    }
}
