using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface ICityService
    {
        Task<City> Save(City city);
        Task<PaginationResult<City>> Pagination(PaginationFilter filter);
        Task<City> GetById(string id);
        Task Delete(string id);
    }
}
