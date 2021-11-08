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
    [Authorize]
    [Route("/air-taxi-bases")]
    public class AirTaxiBaseController : BaseController
    {
        private readonly IAirTaxiBaseService _airTaxiBaseService;

        public AirTaxiBaseController(IDataContext dataContext, IInformer informer, IAirTaxiBaseService airTaxiBaseService) : base(dataContext, informer)
        {
            _airTaxiBaseService = airTaxiBaseService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]AirTaxiBase airTaxiBase)
        {
            GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            User user = _informer.GetValue<User>(UserTypes.User); 

            using var transaction = _dataContext.Database.BeginTransaction();

            airTaxiBase.air_taxi_id = user.air_taxi_id;
            AirTaxiBase _airTaxiBase = await _airTaxiBaseService.Save(airTaxiBase);
            await transaction.CommitAsync();

            return HttpResponse(_airTaxiBase);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AirTaxiBase>> GetById(string id)
        {
            AirTaxiBase airTaxiBase = await _airTaxiBaseService.GetById(id);
            return HttpResponse(airTaxiBase);
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
            User user = _informer.GetValue<User>(UserTypes.User);

            filter.air_taxi_id = user.air_taxi_id;
            PaginationResult<AirTaxiBase> airTaxiBases = await _airTaxiBaseService.Pagination(filter);
            return HttpResponse(airTaxiBases);
        }
    }
}
