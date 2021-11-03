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
    [Route("/customer-addresses")]
    public class CustomerAddressController : BaseController
    {
        private readonly ICustomerAddressService _customerAddressService;

        public CustomerAddressController(IDataContext dataContext, IInformer informer, ICustomerAddressService customerAddressService) : base(dataContext, informer)
        {
            _customerAddressService = customerAddressService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]CustomerAddress customerAddress)
        {
            string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
            using var transaction = _dataContext.Database.BeginTransaction();

            customerAddress.customer_id = customerId;
            customerAddress = await _customerAddressService.Save(customerAddress);

            await transaction.CommitAsync();

            return HttpResponse(customerAddress);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<CustomerAddress> customerAddresses = await _customerAddressService.Pagination(filter);
            
            return HttpResponse(customerAddresses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAddress>> GetById(string id)
        {
            CustomerAddress customerAddress = await _customerAddressService.GetById(id);
            return HttpResponse(customerAddress);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _customerAddressService.Delete(id);
            return HttpResponse();
        }
    }
}
