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
    [Route("/aerodromes")]
    public class AerodromeController : BaseController
    {
        private readonly IAerodromeService _aerodromeService;

        public AerodromeController(
            IDataContext dataContext, 
            INotificator notificator, 
            IInformer informer, 
            IAerodromeService aerodromeService
        ) : base(
            dataContext, 
            notificator, 
            informer
        )
        {
            _aerodromeService = aerodromeService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Aerodrome>> GetById(string id)
        {
            try
            {
                Aerodrome aerodrome = await _aerodromeService.GetById(id);
                return HttpResponse(aerodrome);
            }
            catch(Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Aerodrome aerodrome)
        {
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                aerodrome = await _aerodromeService.Save(aerodrome);
                await transaction.CommitAsync();

                return HttpResponse(aerodrome);
            }
            catch(Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost("external")]
        [AllowAnonymous]
        public async Task<ActionResult> SaveExternal([FromBody]Aerodrome aerodrome)
        {
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                aerodrome = await _aerodromeService.SaveExternal(aerodrome);
                await transaction.CommitAsync();

                return HttpResponse(aerodrome);
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
                PaginationResult<Aerodrome> aerodromes = await _aerodromeService.Pagination(filter);
                return HttpResponse(aerodromes);
            }
            catch(Exception ex)
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
                IEnumerable<Aerodrome> aerodromes = await _aerodromeService.Autocomplete(autocompleteParams);
                return HttpResponse(aerodromes);
            }
            catch(Exception ex)
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
                await _aerodromeService.Delete(id);
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
