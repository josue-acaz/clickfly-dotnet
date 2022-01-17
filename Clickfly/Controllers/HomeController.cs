using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Services;
using Newtonsoft.Json;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
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
        [AllowAnonymous]
        public ActionResult Pagination([FromQuery]PaginationFilter filter)
        {
            return HttpResponse(new { Message = "Clickfly API v1.0" });
        }
    }
}
