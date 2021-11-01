using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Route("/aircraft-images")]
    public class AircraftImageController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly IAircraftImageService _aircraftImageService;

        public AircraftImageController(IDataContext dataContext, IAircraftImageService aircraftImageService)
        {
            _dataContext  = dataContext;
            _aircraftImageService = aircraftImageService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromForm]AircraftImage aircraftImage)
        {
            using var transaction = _dataContext.Database.BeginTransaction();
            AircraftImage _aircraftImage = await _aircraftImageService.Save(aircraftImage);
            await transaction.CommitAsync();

            return HttpResponse(aircraftImage);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Pagination([FromQuery]AircraftImagePaginationFilter filter)
        {
            PaginationResult<AircraftImage> aircraftImages = await _aircraftImageService.Pagination(filter);
            return HttpResponse(aircraftImages);
        }
    }
}
