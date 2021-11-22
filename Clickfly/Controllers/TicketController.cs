using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Authorize]
    [Route("/tickets")]
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;

        public TicketController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            ITicketService ticketService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _ticketService = ticketService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetById(string id)
        {
            Ticket ticket = await _ticketService.GetById(id);
            return HttpResponse(ticket);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]Ticket ticket)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            ticket = await _ticketService.Save(ticket);
            await transaction.CommitAsync();

            return HttpResponse(ticket);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<Ticket> tickets = await _ticketService.Pagination(filter);
            return HttpResponse(tickets);
        }
    }
}
