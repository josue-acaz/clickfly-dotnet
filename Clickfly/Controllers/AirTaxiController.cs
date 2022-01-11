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
    [Route("/air-taxis")]
    public class AirTaxiController : BaseController
    {
        private readonly IAirTaxiService _airTaxiService;

        public AirTaxiController(
            IDataContext dataContext, 
            IInformer informer, 
            INotificator notificator,
            IAirTaxiService airTaxiService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _airTaxiService = airTaxiService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]AirTaxi airTaxi)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            AirTaxi _airTaxi = await _airTaxiService.Save(airTaxi);
            await transaction.CommitAsync();

            return HttpResponse(_airTaxi);
        }

        [HttpGet("getInfo/{token}")]
        public async Task<ActionResult<AirTaxi>> GetInfo(string token)
        {
            AirTaxi airTaxi = await _airTaxiService.GetByAccessToken(token);
            return HttpResponse(airTaxi);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AirTaxi>> GetById(string id)
        {
            AirTaxi airTaxi = await _airTaxiService.GetById(id);
            return HttpResponse(airTaxi);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        { 
            PaginationResult<AirTaxi> airTaxis = await _airTaxiService.Pagination(filter);
            return HttpResponse(airTaxis);
        }
    }
}
