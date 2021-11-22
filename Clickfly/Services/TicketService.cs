using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.ViewModels;
using clickfly.Helpers;
using Microsoft.Extensions.Options;

namespace clickfly.Services
{
    public class TicketService : BaseService, ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(
            IOptions<AppSettings> appSettings, 
            INotificator notificator, 
            IInformer informer,
            IUtils utils, 
            ITicketRepository ticketRepository
        ) : base(
            appSettings, 
            notificator, 
            informer,
            utils
        )
        {
            _ticketRepository = ticketRepository;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetById(string id)
        {
            Ticket ticket = await _ticketRepository.GetById(id);
            return ticket;
        }

        public async Task<PaginationResult<Ticket>> Pagination(PaginationFilter filter)
        {
            PaginationResult<Ticket> paginationResult = await _ticketRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<Ticket> Save(Ticket ticket)
        {
            bool update = ticket.id != "";

            string qr_code = _utils.GenerateQRCode(ticket.qr_code);
            ticket.qr_code = qr_code;

            if(update)
            {
                
            }
            else
            {
                ticket = await _ticketRepository.Create(ticket);
            }

            return ticket;
        }
    }
}
