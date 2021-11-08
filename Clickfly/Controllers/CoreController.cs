using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CoordinateSharp;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Services;

namespace clickfly.Controllers
{
    [Route("/cores")]
    public class CoreController : BaseController
    {
        private readonly ICoreService _coreService;
        private readonly IAerodromeService _aerodromeService;
        private readonly ICepService _cepService;

        public CoreController(IDataContext dataContext, IInformer informer, ICoreService coreService, ICepService cepService, IAerodromeService aerodromeService) : base(dataContext, informer)
        {
            _coreService = coreService;
            _cepService = cepService;
            _aerodromeService = aerodromeService;
        }

        [HttpGet("flight-time")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetFlightTime([FromQuery]FlightTimeRequest flightTimeRequest)
        {
            double flightTime = await _coreService.GetFlightTime(flightTimeRequest);
            return flightTime;
        }

        [HttpGet("flight-price")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetFlightPrice([FromQuery]FlightPriceRequest flightPriceRequest)
        {
            double flightPrice = await _coreService.GetFlightPrice(flightPriceRequest);
            return flightPrice;
        }

        [HttpGet("flight-subtotal")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetFlightSubtotal([FromQuery]FlightSubtotalRequest flightSubtotalRequest)
        {
            Console.WriteLine(flightSubtotalRequest.selected_seats);
            Console.WriteLine(flightSubtotalRequest.flight_segment_id);
            double flightPrice = await _coreService.GetFlightSubtotal(flightSubtotalRequest);
            return flightPrice;
        }

        [HttpGet("distance-between-aerodromes")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetDistanceBetweenAerodromes([FromQuery]DistanceBetweenAerodromesRequest distanceBetweenAerodromesRequest)
        {
            string originAerodromeId = distanceBetweenAerodromesRequest.origin_aerodrome_id;
            string destinationAerodromeId = distanceBetweenAerodromesRequest.destination_aerodrome_id;

            Aerodrome originAerodrome = await _aerodromeService.GetById(originAerodromeId);
            Aerodrome destinationAerodrome = await _aerodromeService.GetById(destinationAerodromeId);

            Coordinate p1 = new Coordinate();
            Coordinate p2 = new Coordinate();

            p1.Latitude = new CoordinatePart(originAerodrome.latitude, CoordinateType.Lat);
            p2.Longitude = new CoordinatePart(originAerodrome.longitude, CoordinateType.Long);

            p2.Latitude = new CoordinatePart(destinationAerodrome.latitude, CoordinateType.Lat);
            p2.Longitude = new CoordinatePart(destinationAerodrome.longitude, CoordinateType.Long);

            double distance = _coreService.GetDistanceBetweenCoordinates(p1, p2);
            return distance;
        }

        [HttpGet("installments")]
        [AllowAnonymous]
        public async Task<ActionResult<Installment[]>> GetInstallments([FromQuery]InstallmentsRequest installmentsRequest)
        {
            Installment[] installments = await _coreService.GetInstallments(installmentsRequest);
            return installments;
        }
    
        [HttpGet("current-datetime")]
        [AllowAnonymous]
        public DateTime GetCurrentDatetime()
        {
            return DateTime.Now;
        }

        [HttpGet("consult-cep/{cep}")]
        [AllowAnonymous]
        public async Task<ActionResult<ConsultCepResponse>> ConsultCep(string cep)
        {
            ConsultCepResponse consultCepResponse = await _cepService.ConsultCep(cep);
            return consultCepResponse;
        }
    }
}
