using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

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
