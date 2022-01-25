using System;
using System.Threading.Tasks;
using clickfly.Models;
using System.Collections.Generic;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IAircraftService
    {
        Task<Aircraft> Save(Aircraft aircraft);
        Task<Aircraft> GetById(string id);
        Task Delete(string id);
        Task<string> GetThumbnail(GetThumbnailRequest thumbnailRequest);
        Task<PaginationResult<Aircraft>> Pagination(PaginationFilter filter);
        Task<IEnumerable<Aircraft>> Autocomplete(AutocompleteParams autocompleteParams);
        Task<string> Thumbnail(ThumbnailRequest thumbnailRequest);
    }
}