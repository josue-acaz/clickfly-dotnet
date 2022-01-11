using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IAircraftRepository
    {
        Task<Aircraft> Create(Aircraft aircraft);
        Task<Aircraft> GetById(string id);
        Task<Aircraft> Update(Aircraft aircraft);
        Task Delete(string id);
        Task<PaginationResult<Aircraft>> Pagination(PaginationFilter filter);
        Task<string> GetThumbnail(GetThumbnailRequest thumbnailRequest);
    }
}
