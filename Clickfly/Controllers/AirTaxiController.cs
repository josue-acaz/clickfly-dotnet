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
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                AirTaxi _airTaxi = await _airTaxiService.Save(airTaxi);
                await transaction.CommitAsync();

                return HttpResponse(_airTaxi);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("getInfo/{token}")]
        public async Task<ActionResult<AirTaxi>> GetInfo(string token)
        {
            try
            {
                AirTaxi airTaxi = await _airTaxiService.GetByAccessToken(token);
                return HttpResponse(airTaxi);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AirTaxi>> GetById(string id)
        {
            try
            {
                AirTaxi airTaxi = await _airTaxiService.GetById(id);
                return HttpResponse(airTaxi);
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
                PaginationResult<AirTaxi> airTaxis = await _airTaxiService.Pagination(filter);
                return HttpResponse(airTaxis);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _airTaxiService.Delete(id);
                return HttpResponse();
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }
    }
}
