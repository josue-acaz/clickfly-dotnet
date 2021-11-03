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
    [Route("/timezones")]
    public class TimezoneController : BaseController
    {
        private readonly ITimezoneService _timezoneService;

        public TimezoneController(IDataContext dataContext, IInformer informer, ITimezoneService timezoneService) : base(dataContext, informer)
        {
            _timezoneService = timezoneService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]Timezone timezone)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            Timezone _timezone = await _timezoneService.Save(timezone);
            await transaction.CommitAsync();

            return HttpResponse(_timezone);
        }
    }
}
