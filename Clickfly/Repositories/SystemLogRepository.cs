using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;

namespace clickfly.Repositories
{
    public class SystemLogRepository : BaseRepository<State>, ISystemLogRepository
    {
        public SystemLogRepository(
            IDBContext dBContext, 
            IDataContext dataContext, 
            IDapperWrapper dapperWrapper, 
            IUtils utils
        ) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<SystemLog> Create(SystemLog systemLog)
        {
            systemLog.id = Guid.NewGuid().ToString();

            List<string> exclude = new List<string>();
            exclude.Add("per_updated_date");
            exclude.Add("per_updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = systemLog;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<SystemLog>(options);
            return systemLog;
        }
    }
}