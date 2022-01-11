using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IFlightService
    {
        Task<Flight> Save(Flight flight);
        Task<PaginationResult<Flight>> Pagination(PaginationFilter filter);
        Task<Flight> GetById(string id);
        Task Delete(string id);
    }
}