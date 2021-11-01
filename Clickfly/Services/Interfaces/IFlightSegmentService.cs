using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface IFlightSegmentService
    {
        Task<FlightSegment> Save(FlightSegment flightSegment);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
        Task<FlightSegment> GetById(string id);
        Task Delete(string id);
    }
}