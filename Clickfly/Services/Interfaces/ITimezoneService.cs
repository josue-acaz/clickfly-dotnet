using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface ITimezoneService
    {
        Task<Timezone> Save(Timezone timezone);
        Task<IEnumerable<Timezone>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<Timezone> GetByGmt(int gmt);
        Task Delete(string id);
    }
}
