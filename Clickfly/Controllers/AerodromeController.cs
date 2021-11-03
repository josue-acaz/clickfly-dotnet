using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Route("/aerodromes")]
    public class AerodromeController : BaseController
    {
        private readonly IAerodromeService _aerodromeService;

        public AerodromeController(IDataContext dataContext, IInformer informer, IAerodromeService aerodromeService) : base(dataContext, informer)
        {
            _aerodromeService = aerodromeService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]Aerodrome aerodrome)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            Aerodrome _aerodrome = await _aerodromeService.Save(aerodrome);
            await transaction.CommitAsync();

            return HttpResponse(_aerodrome);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<Aerodrome> aerodromes = await _aerodromeService.Pagination(filter);
            
            return HttpResponse(aerodromes);
        }

        [HttpGet("autocomplete")]
        [AllowAnonymous]
        public async Task<ActionResult> Autocomplete([FromQuery]AutocompleteParams autocompleteParams)
        {
            IEnumerable<Aerodrome> aerodromes = await _aerodromeService.Autocomplete(autocompleteParams);
            return HttpResponse(aerodromes);
        }
    }
}
