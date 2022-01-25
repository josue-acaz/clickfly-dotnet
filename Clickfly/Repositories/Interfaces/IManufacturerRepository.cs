using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IManufacturerRepository
    {
        Task<Manufacturer> Create(Manufacturer manufacturer);
        Task<Manufacturer> GetById(string id);
        Task<Manufacturer> Update(Manufacturer manufacturer);
        Task Delete(string id);
        Task<PaginationResult<Manufacturer>> Pagination(PaginationFilter filter);
    }
}
