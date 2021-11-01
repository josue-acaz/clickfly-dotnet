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
    [Route("/aircraft-models")]
    public class AircraftModelController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly IAircraftModelService _aircraftModelService;

        public AircraftModelController(IDataContext dataContext, IAircraftModelService aircraftModelService)
        {
            _dataContext  = dataContext;
            _aircraftModelService = aircraftModelService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]AircraftModel aircraftModel)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            AircraftModel _aircraftModel = await _aircraftModelService.Save(aircraftModel);
            await transaction.CommitAsync();

            return HttpResponse(_aircraftModel);
        }

        [HttpGet("autocomplete")]
        [AllowAnonymous]
        public async Task<ActionResult> Autocomplete([FromQuery]AutocompleteParams autocompleteParams)
        {
            IEnumerable<AircraftModel> aircraftModels = await _aircraftModelService.Autocomplete(autocompleteParams);
            return HttpResponse(aircraftModels);
        }
    }
}
