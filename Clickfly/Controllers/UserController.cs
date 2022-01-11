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
            User user = await _userService.GetById(id);
            return HttpResponse(user);
        }

        [HttpPost]
        [AllowAnonymous]
        //[Authorize(Roles = "administrator,general_administrator")]
        public async Task<ActionResult> Save([FromBody]User user)
        {
            //GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            using var transaction = _dataContext.Database.BeginTransaction();

            User _user = await _userService.Save(user);
            await transaction.CommitAsync();

            return HttpResponse(_user);
        }

        [HttpPut("update-role")]
        [Authorize(Roles = "administrator,general_administrator")]
        public async Task<ActionResult> UpdateRole([FromBody]UpdateRole updateRole)
        {
            //GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            using var transaction = _dataContext.Database.BeginTransaction();

            Console.WriteLine("AQUII");

            User _user = await _userService.UpdateRole(updateRole);
            await transaction.CommitAsync();

            return HttpResponse(_user);
        }

        [HttpGet]
        [Authorize(Roles = "general_administrator,administrator")]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        { 
            PaginationResult<User> users = await _userService.Pagination(filter);
            return HttpResponse(users);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<ActionResult<Authenticated>> Authenticate([FromBody]AuthenticateParams authenticateParams)
        {
            Authenticated authenticated = await _userService.Authenticate(authenticateParams);
            return authenticated;
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
    }
}
