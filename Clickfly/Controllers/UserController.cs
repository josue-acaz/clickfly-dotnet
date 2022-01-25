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
    [Route("/users")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IUserService userService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                User user = await _userService.GetById(id);
                return HttpResponse(user);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Authorize(Roles = "administrator,general_administrator")]
        public async Task<ActionResult> Save([FromBody]User user)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                user = await _userService.Save(user);
                await transaction.CommitAsync();

                return HttpResponse(user);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPut("update-role")]
        [Authorize(Roles = "administrator,general_administrator")]
        public async Task<ActionResult> UpdateRole([FromBody]UpdateRole updateRole)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                User _user = await _userService.UpdateRole(updateRole);
                await transaction.CommitAsync();

                return HttpResponse(_user);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet]
        [Authorize(Roles = "general_administrator,administrator")]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        { 
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                PaginationResult<User> users = await _userService.Pagination(filter);
                return HttpResponse(users);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<ActionResult<Authenticated>> Authenticate([FromBody]AuthenticateParams authenticateParams)
        {
            try
            {
                Authenticated authenticated = await _userService.Authenticate(authenticateParams);
                return authenticated;
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult<Authenticated>> ChangePassword([FromBody]ChangePassword changePassword)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                await _userService.ChangePassword(changePassword);
                return HttpResponse();
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _userService.Delete(id);
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
