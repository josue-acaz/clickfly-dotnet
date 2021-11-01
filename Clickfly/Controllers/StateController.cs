using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;

namespace clickfly.Controllers
{
    [Route("/states")]
    public class StateController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly IStateService _stateService;

        public StateController(IDataContext dataContext, IStateService stateService)
        {
            _dataContext  = dataContext;
            _stateService = stateService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<State>> GetById(string id)
        {
            State state = await _stateService.GetById(id);
            return HttpResponse(state);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]State state)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            State _state = await _stateService.Save(state);
            await transaction.CommitAsync();

            return HttpResponse(_state);
        }
    }
}
