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
    [Route("/user-roles")]
    public class UserRoleController : BaseController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IUserRoleService userRoleService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _userRoleService = userRoleService;
        }

        [HttpPost]
        [Authorize(Roles = "general_administrator")]
        public async Task<ActionResult> Save([FromBody]UserRole userRole)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                userRole = await _userRoleService.Save(userRole);
                await transaction.CommitAsync();

                return HttpResponse(userRole);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet]
        [Authorize(Roles = "general_administrator")]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        { 
            try
            {
                PaginationResult<UserRole> userRoles = await _userRoleService.Pagination(filter);
                return HttpResponse(userRoles);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("autocomplete")]
        [Authorize(Roles = "general_administrator,administrator")]
        public async Task<ActionResult> Autocomplete([FromQuery]AutocompleteParams autocompleteParams)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                IEnumerable<UserRole> user_roles = await _userRoleService.Autocomplete(autocompleteParams);
                return HttpResponse(user_roles);
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
                await _userRoleService.Delete(id);
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
