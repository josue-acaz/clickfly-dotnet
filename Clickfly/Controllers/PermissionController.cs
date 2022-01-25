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
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                permission = await _permissionService.Save(permission);
                await transaction.CommitAsync();

                return HttpResponse(permission);
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
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                PaginationResult<Permission> permissions = await _permissionService.Pagination(filter);
                return HttpResponse(permissions);
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
                await _permissionService.Delete(id);
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
