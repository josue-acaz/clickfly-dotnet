using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> Create(Booking booking);
        Task<Booking> GetById(string id);
        Task Update(Booking booking);
        Task Delete(string id);
        Task<PaginationResult<Booking>> Pagination(PaginationFilter filter);
    }
}
