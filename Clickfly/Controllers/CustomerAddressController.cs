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
    [Route("/customer-addresses")]
    public class CustomerAddressController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly ICustomerAddressService _customerAddressService;

        public CustomerAddressController(IDataContext dataContext, ICustomerAddressService customerAddressService)
        {
            _dataContext  = dataContext;
            _customerAddressService = customerAddressService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]CustomerAddress customerAddress)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            CustomerAddress _customerAddress = await _customerAddressService.Save(customerAddress);
            await transaction.CommitAsync();

            return HttpResponse(_customerAddress);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<CustomerAddress> customerAddresses = await _customerAddressService.Pagination(filter);
            
            return HttpResponse(customerAddresses);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerAddress>> GetById(string id)
        {
            CustomerAddress customerAddress = await _customerAddressService.GetById(id);
            return HttpResponse(customerAddress);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete(string id)
        {
            await _customerAddressService.Delete(id);
            return HttpResponse();
        }
    }
}
