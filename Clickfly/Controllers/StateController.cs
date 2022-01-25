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
    [Route("/states")]
    public class StateController : BaseController
    {
        private readonly IStateService _stateService;

        public StateController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IStateService stateService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _stateService = stateService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetById(string id)
        {
            try
            {
                State state = await _stateService.GetById(id);
                return HttpResponse(state);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]State state)
        {
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                state = await _stateService.Save(state);
                await transaction.CommitAsync();

                return HttpResponse(state);
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
                IEnumerable<State> states = await _stateService.Autocomplete(autocompleteParams);
                return HttpResponse(states);
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
                await _stateService.Delete(id);
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
