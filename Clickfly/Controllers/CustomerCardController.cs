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
    [Authorize]
    [Route("/customer-cards")]
    public class CustomerCardController : BaseController
    {
        private readonly ICustomerCardService _customerCardService;

        public CustomerCardController(IDataContext dataContext, IInformer informer, ICustomerCardService customerCardService) : base(dataContext, informer)
        {
            _customerCardService = customerCardService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]CustomerCard customerCard)
        {
            string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
            using var transaction = _dataContext.Database.BeginTransaction();

            customerCard.customer_id = customerId;
            customerCard = await _customerCardService.Save(customerCard);
            await transaction.CommitAsync();

            return HttpResponse(customerCard);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
            
            filter.customer_id = customerId;
            PaginationResult<CustomerCard> customerCards = await _customerCardService.Pagination(filter);
            return HttpResponse(customerCards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerCard>> GetById(string id)
        {
            CustomerCard customerCard = await _customerCardService.GetById(id);
            return HttpResponse(customerCard);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _customerCardService.Delete(id);
            return HttpResponse();
        }
    }
}
