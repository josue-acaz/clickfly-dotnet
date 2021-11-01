using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IPassengerRepository
    {
        Task<Passenger> Create(Passenger passenger);
        Task RangeCreate(Passenger[] passengers);
        Task<Passenger> GetById(string id);
        Task<Passenger> Update(Passenger passenger, string[] fields = null);
        Task Delete(string id);
        Task<PaginationResult<Passenger>> Pagination(PaginationFilter filter);
    }
}
