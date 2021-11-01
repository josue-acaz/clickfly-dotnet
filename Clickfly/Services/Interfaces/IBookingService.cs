using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface IBookingService
    {
        Task<Booking> Save(Booking booking);
        Task<Booking> GetById(string id);
        Task Delete(string id);
        Task<PaginationResult<Booking>> Pagination(PaginationFilter filter);
    }
}
