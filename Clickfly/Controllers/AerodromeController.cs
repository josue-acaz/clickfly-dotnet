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

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]Aerodrome aerodrome)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            Aerodrome _aerodrome = await _aerodromeService.Save(aerodrome);
            await transaction.CommitAsync();

            return HttpResponse(_aerodrome);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<Aerodrome> aerodromes = await _aerodromeService.Pagination(filter);
            return HttpResponse(aerodromes);
        }

        [HttpGet("autocomplete")]
        public async Task<ActionResult> Autocomplete([FromQuery]AutocompleteParams autocompleteParams)
        {
            IEnumerable<Aerodrome> aerodromes = await _aerodromeService.Autocomplete(autocompleteParams);
            return HttpResponse(aerodromes);
        }
    }
}
