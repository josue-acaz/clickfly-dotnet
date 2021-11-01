using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace clickfly.Repositories
{
    public class AircraftModelRepository : BaseRepository<AircraftModel>, IAircraftModelRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "aircraft_models as aircraft_model";
        private static string whereSql = "aircraft_model.excluded = false";
        protected string[] defaultFields = new string[8];

        public AircraftModelRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils) : base(dBContext, dataContext, utils)
        {
            
        }

        public async Task<AircraftModel> Create(AircraftModel aircraftModel)
        {
            string id = Guid.NewGuid().ToString();
            aircraftModel.id = id;

            await _dataContext.AircraftModels.AddAsync(aircraftModel);
            await _dataContext.SaveChangesAsync();

            return aircraftModel;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AircraftModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<AircraftModel>> Pagination(PaginationFilter filter)
        {
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            List<AircraftModel> aircraftModels = await _dataContext.AircraftModels
               .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
               .Take(paginationFilter.page_size)
               .ToListAsync();
            
            int total_records = await _dataContext.AircraftModels.CountAsync();
            PaginationResult<AircraftModel> paginationResult = _utils.CreatePaginationResult<AircraftModel>(aircraftModels, paginationFilter, total_records);

            return paginationResult;
        }

        public Task<AircraftModel> Update(AircraftModel aircraftModel, string[] fields = null)
        {
            throw new NotImplementedException();
        }
    }
}