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
    [Route("/aircraft-models")]
    public class AircraftModelController : BaseController
    {
        private readonly IAircraftModelService _aircraftModelService;

        public AircraftModelController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IAircraftModelService aircraftModelService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _aircraftModelService = aircraftModelService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]AircraftModel aircraftModel)
        {
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                AircraftModel _aircraftModel = await _aircraftModelService.Save(aircraftModel);
                await transaction.CommitAsync();

                return HttpResponse(_aircraftModel);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AircraftModel>> GetById(string id)
        {
            try
            {
                AircraftModel aircraftModel = await _aircraftModelService.GetById(id);
                return HttpResponse(aircraftModel);
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
                PaginationResult<AircraftModel> aircraftModels = await _aircraftModelService.Pagination(filter);
                return HttpResponse(aircraftModels);
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
                IEnumerable<AircraftModel> aircraftModels = await _aircraftModelService.Autocomplete(autocompleteParams);
                return HttpResponse(aircraftModels);
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
                await _aircraftModelService.Delete(id);
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
