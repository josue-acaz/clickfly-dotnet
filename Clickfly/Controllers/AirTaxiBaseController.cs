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
    [Route("/air-taxi-bases")]
    public class AirTaxiBaseController : BaseController
    {
        private readonly IAirTaxiBaseService _airTaxiBaseService;

        public AirTaxiBaseController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IAirTaxiBaseService airTaxiBaseService
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _airTaxiBaseService = airTaxiBaseService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromBody]AirTaxiBase airTaxiBase)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User); 
                using var transaction = _dataContext.Database.BeginTransaction();

                AirTaxiBase _airTaxiBase = await _airTaxiBaseService.Save(airTaxiBase);
                await transaction.CommitAsync();

                return HttpResponse(_airTaxiBase);
            }
            catch(Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AirTaxiBase>> GetById(string id)
        {
            try
            {
                AirTaxiBase airTaxiBase = await _airTaxiBaseService.GetById(id);
                return HttpResponse(airTaxiBase);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]PaginationFilter filter)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                
                PaginationResult<AirTaxiBase> airTaxiBases = await _airTaxiBaseService.Pagination(filter);
                return HttpResponse(airTaxiBases);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }
    }
}
