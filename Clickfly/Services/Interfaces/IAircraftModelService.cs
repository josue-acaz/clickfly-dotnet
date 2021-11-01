using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;
using System.Collections.Generic;

namespace clickfly.Services
{
    public interface IAircraftModelService
    {
        Task<AircraftModel> Save(AircraftModel aircraftModel);
        Task<PaginationResult<AircraftModel>> Pagination(PaginationFilter filter);
        Task<IEnumerable<AircraftModel>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<AircraftModel> GetById(string id);
        Task Delete(string id);
    }
}
