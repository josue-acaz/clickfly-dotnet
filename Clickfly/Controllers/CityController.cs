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
    [Route("/cities")]
    public class CityController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly ICityService _cityService;

        public CityController(IDataContext dataContext, ICityService cityService)
        {
            _dataContext  = dataContext;
            _cityService = cityService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]City city)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            City _city = await _cityService.Save(city);
            await transaction.CommitAsync();

            return HttpResponse(_city);
        }
    }
}
