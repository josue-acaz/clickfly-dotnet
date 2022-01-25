using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IBookingPaymentRepository
    {
        Task<BookingPayment> Create(BookingPayment bookingPayment);
        Task<BookingPayment> GetById(string id);
        Task<BookingPayment> Update(BookingPayment bookingPayment);
        Task Delete(string id);
        Task<PaginationResult<BookingPayment>> Pagination(PaginationFilter filter);
    }
}
