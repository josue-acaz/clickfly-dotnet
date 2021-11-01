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
    [Route("/customer-friends")]
    public class CustomerFriendController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly ICustomerFriendService _customerFriendService;

        public CustomerFriendController(IDataContext dataContext, ICustomerFriendService customerFriendService)
        {
            _dataContext  = dataContext;
            _customerFriendService = customerFriendService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]CustomerFriend customerFriend)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            CustomerFriend _customerFriend = await _customerFriendService.Save(customerFriend);
            await transaction.CommitAsync();

            return HttpResponse(_customerFriend);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<CustomerFriend> customerFriends = await _customerFriendService.Pagination(filter);
            
            return HttpResponse(customerFriends);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerFriend>> GetById(string id)
        {
            CustomerFriend customerFriend = await _customerFriendService.GetById(id);
            return HttpResponse(customerFriend);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete(string id)
        {
            await _customerFriendService.Delete(id);
            return HttpResponse();
        }
    }
}
