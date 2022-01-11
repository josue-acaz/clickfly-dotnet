using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IFlightSegmentService
    {
        Task<FlightSegment> Save(FlightSegment flightSegment);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
        Task<FlightSegment> GetById(string id);
        Task Delete(string id);
        Task SendNotification(string id);
    }
}