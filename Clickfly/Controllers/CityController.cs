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
    [Route("/cities")]
    public class CityController : BaseController
    {
        private readonly ICityService _cityService;

        public CityController(
            IDataContext dataContext, 
            IInformer informer, 
            INotificator notificator,
            ICityService cityService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _cityService = cityService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]City city)
        {
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                city = await _cityService.Save(city);
                await transaction.CommitAsync();

                return HttpResponse(city);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost("external")]
        [AllowAnonymous]
        public async Task<ActionResult> SaveExternal([FromBody]City city)
        {
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                city = await _cityService.SaveExternal(city);
                await transaction.CommitAsync();

                return HttpResponse(city);
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
                PaginationResult<City> cities = await _cityService.Pagination(filter);
                
                return HttpResponse(cities);
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
                IEnumerable<City> cities = await _cityService.Autocomplete(autocompleteParams);
                return HttpResponse(cities);
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
                await _cityService.Delete(id);
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
