using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;

namespace clickfly.Services
{
    public class FlightSegmentService : IFlightSegmentService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IFlightSegmentRepository _flightSegmentRepository;

        public FlightSegmentService(IFlightRepository flightRepository, IFlightSegmentRepository flightSegmentRepository)
        {
            _flightRepository = flightRepository;
            _flightSegmentRepository = flightSegmentRepository;
        }

        public async Task Delete(string id)
        {
            await _flightRepository.Delete(id);
        }

        public async Task<FlightSegment> GetById(string id)
        {
            FlightSegment flightSegment = await _flightSegmentRepository.GetById(id);
            return flightSegment;
        }

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            PaginationResult<FlightSegment> paginationResult = await _flightSegmentRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<FlightSegment> Save(FlightSegment flightSegment)
        {
            bool update = flightSegment.id != "";

            if(update)
            {
                flightSegment = await _flightSegmentRepository.Update(flightSegment);
            }
            else
            {
                string flightId = flightSegment.flight_id;
                FlightSegment lastSegment = await _flightRepository.GetLastSegment(flightId);
                
                int number = 1;
                if(lastSegment != null)
                {
                    number += lastSegment.number;
                }
                
                flightSegment.number = number;
                flightSegment = await _flightSegmentRepository.Create(flightSegment);
            }

            return flightSegment;
        }
    }
}