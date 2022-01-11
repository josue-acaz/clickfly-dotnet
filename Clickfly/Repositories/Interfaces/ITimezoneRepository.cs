using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface ITimezoneRepository
    {
        Task<Timezone> Create(Timezone timezone);
        Task<Timezone> GetById(string id);
        Task<Timezone> GetByGmt(int gmt);
        Task Update(Timezone timezone);
        Task Delete(string id);
        Task<PaginationResult<Timezone>> Pagination(PaginationFilter filter);
    }
}
