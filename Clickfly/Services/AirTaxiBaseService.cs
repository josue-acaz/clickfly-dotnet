using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;

namespace clickfly.Services
{
    public class AirTaxiBaseService : IAirTaxiBaseService
    {
        private readonly IAirTaxiBaseRepository _airTaxiBaseRepository;

        public AirTaxiBaseService(IAirTaxiBaseRepository airTaxiBaseRepository)
        {
            _airTaxiBaseRepository = airTaxiBaseRepository;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AirTaxiBase> GetById(string id)
        {
            AirTaxiBase airTaxiBase = await _airTaxiBaseRepository.GetById(id);
            return airTaxiBase;
        }

        public async Task<PaginationResult<AirTaxiBase>> Pagination(PaginationFilter filter)
        {
            PaginationResult<AirTaxiBase> paginationResult = await _airTaxiBaseRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<AirTaxiBase> Save(AirTaxiBase airTaxiBase)
        {
            bool update = airTaxiBase.id != "";

            if(update)
            {
                airTaxiBase = await _airTaxiBaseRepository.Update(airTaxiBase);
            }
            else
            {
                airTaxiBase = await _airTaxiBaseRepository.Create(airTaxiBase);
            }

            return airTaxiBase;
        }
    }
}
