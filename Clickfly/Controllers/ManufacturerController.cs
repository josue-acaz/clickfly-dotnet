using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Services;
using Microsoft.AspNetCore.Http;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Authorize]
    [Route("/manufacturers")]
    public class ManufacturerController : BaseController
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(
            IDataContext dataContext, 
            INotificator notificator, 
            IInformer informer, 
            IManufacturerService manufacturerService
        ) : base(
            dataContext, 
            notificator, 
            informer
        )
        {
            _manufacturerService = manufacturerService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Manufacturer manufacturer)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                manufacturer = await _manufacturerService.Save(manufacturer);
                await transaction.CommitAsync();

                return HttpResponse(manufacturer);
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
                PaginationResult<Manufacturer> manufacturers = await _manufacturerService.Pagination(filter);
                
                return HttpResponse(manufacturers);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("autocomplete")]
        public async Task<ActionResult> Autocomplete([FromQuery]AutocompleteParams autocompleteParams)
        {
            try
            {
                IEnumerable<Manufacturer> manufacturers = await _manufacturerService.Autocomplete(autocompleteParams);
            
                return HttpResponse(manufacturers);
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
                await _manufacturerService.Delete(id);
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
