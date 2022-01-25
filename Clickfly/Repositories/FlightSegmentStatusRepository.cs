using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using Npgsql;
using System.Collections.Generic;
using Dapper;
using clickfly.ViewModels;
using System.Linq;

namespace clickfly.Repositories
{
    public class FlightSegmentStatusRepository : BaseRepository<FlightSegmentStatus>, IFlightSegmentStatusRepository
    {
        public FlightSegmentStatusRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<FlightSegmentStatus> Create(FlightSegmentStatus flightSegmentStatus)
        {
            flightSegmentStatus.id = Guid.NewGuid().ToString();
            flightSegmentStatus.created_at = DateTime.Now;
            flightSegmentStatus.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = flightSegmentStatus;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<FlightSegmentStatus>(options);
            return flightSegmentStatus;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<FlightSegmentStatus> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<FlightSegmentStatus>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<FlightSegmentStatus> Update(FlightSegmentStatus flightSegmentStatus)
        {
            throw new NotImplementedException();
        }
    }
}