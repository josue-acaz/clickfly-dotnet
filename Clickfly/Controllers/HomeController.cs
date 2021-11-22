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
using Newtonsoft.Json;

namespace clickfly.Controllers
{
    [Authorize]
    [Route("/home")]
    public class HomeController : BaseController
    {
        public HomeController(
            IDataContext dataContext, 
            IInformer informer, 
            INotificator notificator
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {

        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);

            return HttpResponse(new { Message = "Ok" });
        }
    }
}