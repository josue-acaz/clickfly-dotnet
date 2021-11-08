using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> Create(Ticket ticket);
        Task<Ticket> GetById(string id);
        Task Update(Ticket ticket);
        Task Delete(string id);
        Task<PaginationResult<Ticket>> Pagination(PaginationFilter filter);
    }
}
