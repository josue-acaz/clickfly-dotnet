using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IFlightSegmentRepository
    {
        Task<FlightSegment> Create(FlightSegment flightSegment);
        Task<FlightSegment> GetById(string id);
        Task<FlightSegment> Update(FlightSegment flightSegment, string[] fields = null);
        Task Delete(string id);
        Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter);
    }
}
