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
    [Route("/customer-addresses")]
    public class CustomerAddressController : BaseController
    {
        private readonly ICustomerAddressService _customerAddressService;

        public CustomerAddressController(
            IDataContext dataContext, 
            IInformer informer, 
            INotificator notificator,
            ICustomerAddressService customerAddressService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _customerAddressService = customerAddressService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]CustomerAddress customerAddress)
        {
            try
            {
                string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
                using var transaction = _dataContext.Database.BeginTransaction();

                customerAddress.customer_id = customerId;
                customerAddress = await _customerAddressService.Save(customerAddress);

                await transaction.CommitAsync();

                return HttpResponse(customerAddress);
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
                string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);

                filter.customer_id = customerId;
                PaginationResult<CustomerAddress> customerAddresses = await _customerAddressService.Pagination(filter);
                
                return HttpResponse(customerAddresses);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAddress>> GetById(string id)
        {
            try
            {
                CustomerAddress customerAddress = await _customerAddressService.GetById(id);
                return HttpResponse(customerAddress);
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
                await _customerAddressService.Delete(id);
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
