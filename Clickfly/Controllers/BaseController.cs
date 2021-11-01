using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using clickfly.Data;

namespace clickfly.Controllers
{
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected ActionResult HttpResponse(object result = null)
        {
            return Ok(result);
        }
    }
}