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
    [Route("/double-checks")]
    public class DoubleCheckController : BaseController
    {
        private readonly IDoubleCheckService _doubleCheckService;

        public DoubleCheckController(
            IDataContext dataContext, 
            IInformer informer, 
            INotificator notificator,
            IDoubleCheckService doubleCheckService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _doubleCheckService = doubleCheckService;
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            try
            {
                PaginationResult<DoubleCheck> doubleChecks = await _doubleCheckService.Pagination(filter);
                return HttpResponse(doubleChecks);
            }
            catch(Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }
    }
}
