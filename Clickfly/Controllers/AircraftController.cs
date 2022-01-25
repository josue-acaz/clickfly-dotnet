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
    [Route("/aircrafts")]
    public class AircraftController : BaseController
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(
            IDataContext dataContext, 
            INotificator notificator, 
            IInformer informer, 
            IAircraftService aircraftService
        ) : base(
            dataContext, 
            notificator, 
            informer
        )
        {
            _aircraftService = aircraftService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Aircraft aircraft)
        {  
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();

                aircraft = await _aircraftService.Save(aircraft);
                await transaction.CommitAsync();

                return HttpResponse(aircraft);
            }
            catch(Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Aircraft>> GetById(string id)
        {
            try
            {
                Aircraft aircraft = await _aircraftService.GetById(id);
                return HttpResponse(aircraft);
            }
            catch(Exception ex)
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
                User user = _informer.GetValue<User>(UserTypes.User); 

                filter.air_taxi_id = user.air_taxi_id;
                PaginationResult<Aircraft> aircrafts = await _aircraftService.Pagination(filter);
                
                return HttpResponse(aircrafts);
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
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                IEnumerable<Aircraft> aircrafts = await _aircraftService.Autocomplete(autocompleteParams);
                
                return HttpResponse(aircrafts);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost("thumbnail")]
        public async Task<ActionResult<string>> Thumbnail([FromForm]ThumbnailRequest thumbnailRequest)
        {
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                string url = await _aircraftService.Thumbnail(thumbnailRequest);
                await transaction.CommitAsync();

                return HttpResponse(url);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("thumbnail")]
        public async Task<ActionResult<string>> GetThumbnail([FromQuery]GetThumbnailRequest thumbnailRequest)
        {
            try
            {
                string thumbnail = await _aircraftService.GetThumbnail(thumbnailRequest);
                return HttpResponse(thumbnail);
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
                await _aircraftService.Delete(id);
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
