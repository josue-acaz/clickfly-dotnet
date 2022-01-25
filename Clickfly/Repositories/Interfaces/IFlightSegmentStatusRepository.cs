using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IFlightSegmentStatusRepository
    {
        Task<FlightSegmentStatus> Create(FlightSegmentStatus flightSegmentStatus);
        Task<FlightSegmentStatus> GetById(string id);
        Task<FlightSegmentStatus> Update(FlightSegmentStatus flightSegmentStatus);
        Task Delete(string id);
        Task<PaginationResult<FlightSegmentStatus>> Pagination(PaginationFilter filter);
    }
}
