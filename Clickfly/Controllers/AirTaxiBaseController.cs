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
    [Route("/air-taxi-bases")]
    public class AirTaxiBaseController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly IAirTaxiBaseService _airTaxiBaseService;

        public AirTaxiBaseController(IDataContext dataContext, IAirTaxiBaseService airTaxiBaseService)
        {
            _dataContext  = dataContext;
            _airTaxiBaseService = airTaxiBaseService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]AirTaxiBase airTaxiBase)
        {
            string air_taxi_id = Request.Headers["air_taxi_id"];
            using var transaction = _dataContext.Database.BeginTransaction();

            airTaxiBase.air_taxi_id = air_taxi_id;
            AirTaxiBase _airTaxiBase = await _airTaxiBaseService.Save(airTaxiBase);
            await transaction.CommitAsync();

            return HttpResponse(_airTaxiBase);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AirTaxiBase>> GetById(string id)
        {
            AirTaxiBase airTaxiBase = await _airTaxiBaseService.GetById(id);
            return HttpResponse(airTaxiBase);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            PaginationResult<AirTaxiBase> airTaxiBases = await _airTaxiBaseService.Pagination(filter);
            
            return HttpResponse(airTaxiBases);
        }
    }
}
