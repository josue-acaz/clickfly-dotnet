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
    [Route("/permission-resources")]
    public class PermissionResourceController : BaseController
    {
        private readonly IPermissionResourceService _permissionResourceService;

        public PermissionResourceController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IPermissionResourceService permissionResourceService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _permissionResourceService = permissionResourceService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]PermissionResource permissionResource)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                permissionResource = await _permissionResourceService.Save(permissionResource);
                await transaction.CommitAsync();

                return HttpResponse(permissionResource);
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
                IEnumerable<PermissionResource> permission_resources = await _permissionResourceService.Autocomplete(autocompleteParams);
                return HttpResponse(permission_resources);
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
                await _permissionResourceService.Delete(id);
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
