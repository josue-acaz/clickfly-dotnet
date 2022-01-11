using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface ITicketService
    {
        Task<Ticket> Save(Ticket ticket);
        Task<Ticket> GetById(string id);
        Task<PaginationResult<Ticket>> Pagination(PaginationFilter filter);
        Task Delete(string id);
    }
}
