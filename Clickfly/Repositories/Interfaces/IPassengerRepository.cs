using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IPassengerRepository
    {
        Task<Passenger> Create(Passenger passenger);
        Task RangeCreate(Passenger[] passengers);
        Task<Passenger> GetById(string id);
        Task<Passenger> Update(Passenger passenger);
        Task Delete(string id);
        Task<PaginationResult<Passenger>> Pagination(PaginationFilter filter);
    }
}
