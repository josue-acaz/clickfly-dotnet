using System;
using CoordinateSharp;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using clickfly.Repositories;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Exceptions;

namespace clickfly.Services
{
    public class CoreService : BaseService, ICoreService
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IFlightSegmentRepository _flightSegmentRepository;

        public CoreService(IOptions<AppSettings> appSettings, IUtils utils, IAircraftRepository aircraftRepository, IFlightSegmentRepository flightSegmentRepository) : base(appSettings, utils)
        {
            _aircraftRepository = aircraftRepository;
            _flightSegmentRepository = flightSegmentRepository;
        }

        public double GetDistanceBetweenCoordinates(Coordinate p1, Coordinate p2)
        {
            Distance distance = new Distance(p1, p2);
            double kmDistance = distance.Kilometers;

            return kmDistance;
        }

        public async Task<float> GetFlightPrice(FlightPriceRequest flightPriceRequest)
        {
            int quantity = flightPriceRequest.quantity;
            string flight_segment_id = flightPriceRequest.flight_segment_id;

            FlightSegment flightSegment = await _flightSegmentRepository.GetById(flight_segment_id);
            float price_per_seat = flightSegment.price_per_seat;

            float flight_price = quantity * price_per_seat;

            return flight_price;
        }

        public async Task<double> GetFlightTime(FlightTimeRequest flightTimeRequest)
        {
            double distance = flightTimeRequest.distance;
            string aircraft_id = flightTimeRequest.aircraft_id;

            Aircraft aircraft = await _aircraftRepository.GetById(aircraft_id);

            double flightTime = distance / aircraft.cruising_speed;
            return flightTime;
        }

        public async Task<Installment[]> GetInstallments(InstallmentsRequest installmentsRequest)
        {
            int selected_seats = installmentsRequest.selected_seats;
            string flight_segment_id = installmentsRequest.flight_segment_id;

            FlightSegment flightSegment = await _flightSegmentRepository.GetById(flight_segment_id);
            int available_seats = flightSegment.available_seats;
            float price_per_seat = flightSegment.price_per_seat;

            if(selected_seats > available_seats)
            {
                throw new BadRequestException("O número de assentos selecionados é maior que o disponível.");
            }

            float subtotal = selected_seats * price_per_seat;
            Installment[] installments = _utils.GetInstallments(subtotal);

            return installments;
        }
    
        public async Task<double> GetFlightSubtotal(FlightSubtotalRequest flightSubtotalRequest)
        {
            double selected_seats = flightSubtotalRequest.selected_seats;
            string flight_segment_id = flightSubtotalRequest.flight_segment_id;

            Console.WriteLine(flight_segment_id);

            FlightSegment flightSegment = await _flightSegmentRepository.GetById(flight_segment_id);
            
            if(flightSegment == null)
            {
                throw new NotFoundException("Voo não encontrado.");
            }

            double subtotal = selected_seats * flightSegment.price_per_seat;
            return subtotal;
        }
    }
}
