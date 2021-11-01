using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IBookingStatusRepository
    {
        Task<BookingStatus> Create(BookingStatus bookingStatus);
        Task<BookingStatus> GetById(string id);
        Task Update(BookingStatus bookingStatus);
        Task Delete(string id);
        Task<PaginationResult<BookingStatus>> Pagination(PaginationFilter filter);
    }
}
