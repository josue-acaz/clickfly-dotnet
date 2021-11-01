using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Route("/customer-cards")]
    public class CustomerCardController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly ICustomerCardService _customerCardService;

        public CustomerCardController(IDataContext dataContext, ICustomerCardService customerCardService)
        {
            _dataContext  = dataContext;
            _customerCardService = customerCardService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]CustomerCard customerCard)
        {
            string customer_id = Request.Headers["customer_id"];
            using var transaction = _dataContext.Database.BeginTransaction();

            CustomerCard _customerCard = await _customerCardService.Save(customerCard);
            await transaction.CommitAsync();

            return HttpResponse(_customerCard);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<CustomerCard> customerCards = await _customerCardService.Pagination(filter);
            return HttpResponse(customerCards);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerCard>> GetById(string id)
        {
            CustomerCard customerCard = await _customerCardService.GetById(id);
            return HttpResponse(customerCard);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete(string id)
        {
            await _customerCardService.Delete(id);
            return HttpResponse();
        }
    }
}
