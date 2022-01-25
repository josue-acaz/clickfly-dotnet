using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> Create(Ticket ticket);
        Task<Ticket> GetById(string id);
        Task<Ticket> Update(Ticket ticket);
        Task Delete(string id);
        Task<PaginationResult<Ticket>> Pagination(PaginationFilter filter);
    }
}
