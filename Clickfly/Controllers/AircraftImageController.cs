using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using clickfly.Data;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Authorize]
    [Route("/aircraft-images")]
    public class AircraftImageController : BaseController
    {
        private readonly IAircraftImageService _aircraftImageService;

        public AircraftImageController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            IAircraftImageService aircraftImageService
        ) : base(
            dataContext, 
            notificator, 
            informer
        )
        {
            _aircraftImageService = aircraftImageService;
        }

        [HttpPost]
        public async Task<ActionResult> Save([FromForm]AircraftImage aircraftImage)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                using var transaction = _dataContext.Database.BeginTransaction();
            
                aircraftImage = await _aircraftImageService.Save(aircraftImage);
                await transaction.CommitAsync();

                return HttpResponse(aircraftImage);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Pagination([FromQuery]AircraftImagePaginationFilter filter)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                PaginationResult<AircraftImage> aircraftImages = await _aircraftImageService.Pagination(filter);
                return HttpResponse(aircraftImages);
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
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.User);
                await _aircraftImageService.Delete(id);
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
