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
    [Route("/customer-friends")]
    public class CustomerFriendController : BaseController
    {
        private readonly ICustomerFriendService _customerFriendService;

        public CustomerFriendController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            ICustomerFriendService customerFriendService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _customerFriendService = customerFriendService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]CustomerFriend customerFriend)
        {
            try
            {
                string customerId = GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
                using var transaction = _dataContext.Database.BeginTransaction();

                customerFriend.customer_id = customerId;
                customerFriend = await _customerFriendService.Save(customerFriend);
                await transaction.CommitAsync();

                return HttpResponse(customerFriend);
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
                PaginationResult<CustomerFriend> customerFriends = await _customerFriendService.Pagination(filter);
                return HttpResponse(customerFriends);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerFriend>> GetById(string id)
        {
            try
            {
                CustomerFriend customerFriend = await _customerFriendService.GetById(id);
                return HttpResponse(customerFriend);
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
                await _customerFriendService.Delete(id);
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
