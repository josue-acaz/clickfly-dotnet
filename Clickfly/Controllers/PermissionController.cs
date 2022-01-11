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
    [Route("/permissions")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IPermissionService permissionService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Permission permission)
        {  
            GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            using var transaction = _dataContext.Database.BeginTransaction();

            permission = await _permissionService.Save(permission);
            await transaction.CommitAsync();

            return HttpResponse(permission);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        { 
            GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            PaginationResult<Permission> permissions = await _permissionService.Pagination(filter);
            return HttpResponse(permissions);
        }
    }
}
