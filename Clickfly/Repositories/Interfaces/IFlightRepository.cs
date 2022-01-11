using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IFlightRepository
    {
        Task<Flight> Create(Flight flight);
        Task<Flight> GetById(string id);
        Task<FlightSegment> GetLastSegment(string flightId);
        Task Update(Flight flight);
        Task Delete(string id);
        Task<PaginationResult<Flight>> Pagination(PaginationFilter filter);
    }
}