using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using clickfly.ViewModels;
using Microsoft.AspNetCore.Http;

namespace clickfly.Controllers
{
    [Route("/aircrafts")]
    public class AircraftController : BaseController
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IDataContext dataContext, IInformer informer, IAircraftService aircraftService) : base(dataContext, informer)
        {
            _aircraftService = aircraftService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]Aircraft aircraft)
        {
            string air_taxi_id = Request.Headers["air_taxi_id"];
            using var transaction = _dataContext.Database.BeginTransaction();

            aircraft.air_taxi_id = air_taxi_id;
            Aircraft _aircraft = await _aircraftService.Save(aircraft);
            await transaction.CommitAsync();

            return HttpResponse(aircraft);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Aircraft>> GetById(string id)
        {
            Aircraft aircraft = await _aircraftService.GetById(id);
            return HttpResponse(aircraft);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<Aircraft> aircrafts = await _aircraftService.Pagination(filter);
            
            return HttpResponse(aircrafts);
        }

        [HttpGet("autocomplete")]
        [AllowAnonymous]
        public async Task<ActionResult> Autocomplete([FromQuery]AutocompleteParams autocompleteParams)
        {
            IEnumerable<Aircraft> aircrafts = await _aircraftService.Autocomplete(autocompleteParams);
            return HttpResponse(aircrafts);
        }

        [HttpPost("thumbnail")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Thumbnail([FromForm]ThumbnailRequest thumbnailRequest)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            string url = await _aircraftService.Thumbnail(thumbnailRequest);
            await transaction.CommitAsync();

            return HttpResponse(url);
        }

        [HttpGet("thumbnail")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetThumbnail([FromQuery]GetThumbnailRequest thumbnailRequest)
        {
            string thumbnail = await _aircraftService.GetThumbnail(thumbnailRequest);
            return HttpResponse(thumbnail);
        }
    }
}
