using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

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
