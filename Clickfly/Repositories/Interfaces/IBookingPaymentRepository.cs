using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IBookingPaymentRepository
    {
        Task<BookingPayment> Create(BookingPayment bookingPayment);
        Task<BookingPayment> GetById(string id);
        Task Update(BookingPayment booking);
        Task Delete(string id);
        Task<PaginationResult<BookingPayment>> Pagination(PaginationFilter filter);
    }
}
