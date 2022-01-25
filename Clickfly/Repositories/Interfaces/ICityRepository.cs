using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface ICityRepository
    {
        Task<City> Create(City city);
        Task<City> GetById(string id);
        Task<City> GetByName(string name, string statePrefix);
        Task<City> Update(City city);
        Task Delete(string id);
        Task<PaginationResult<City>> Pagination(PaginationFilter filter);
    }
}
