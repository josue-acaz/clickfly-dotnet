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
    [Route("/customer-cards")]
    public class CustomerCardController : BaseController
    {
        private readonly ICustomerCardService _customerCardService;

        public CustomerCardController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            ICustomerCardService customerCardService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _customerCardService = customerCardService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]CustomerCard customerCard)
        {
            try
            {
                string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
                using var transaction = _dataContext.Database.BeginTransaction();

                customerCard.customer_id = customerId;
                customerCard = await _customerCardService.Save(customerCard);
                await transaction.CommitAsync();

                return HttpResponse(customerCard);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
                PaginationResult<CustomerCard> customerCards = await _customerCardService.Pagination(filter);
                return HttpResponse(customerCards);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerCard>> GetById(string id)
        {
            try
            {
                CustomerCard customerCard = await _customerCardService.GetById(id);
                return HttpResponse(customerCard);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _customerCardService.Delete(id);
                return HttpResponse();
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }
    }
}
