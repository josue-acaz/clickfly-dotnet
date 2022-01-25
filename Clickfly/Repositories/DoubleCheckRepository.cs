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
    public class DoubleCheckRepository : BaseRepository<DoubleCheck>, IDoubleCheckRepository
    {
        public DoubleCheckRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {
        }

        public async Task Approve(DoubleCheck doubleCheck)
        {
            string querySql = $"UPDATE double_checks SET approved = @approved, message = @message, user_id = @user_id, approver_id = @approver_id, WHERE id = @id";
            await _dBContext.GetConnection().ExecuteAsync(querySql, doubleCheck, _dBContext.GetTransaction());
        }

        public async Task<DoubleCheck> Create(DoubleCheck doubleCheck)
        {
            doubleCheck.id = Guid.NewGuid().ToString();
            doubleCheck.created_at = DateTime.Now;
            doubleCheck.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = doubleCheck;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<DoubleCheck>(options);

            return doubleCheck;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DoubleCheck> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DoubleCheck> GetByName(string name, string statePrefix)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<DoubleCheck>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<DoubleCheck> Update(DoubleCheck doubleCheck)
        {
            throw new NotImplementedException();
        }
    }
}