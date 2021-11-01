using System.Threading.Tasks;
using Dapper;
using clickfly.Models;
using clickfly.Data;

namespace clickfly.Repositories
{
    public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IDBContext _dBContext;
        protected readonly IDataContext _dataContext;
        protected readonly IUtils _utils;

        public BaseRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils)
        {
            _dBContext = dBContext;
            _dataContext = dataContext;
            _utils = utils;
        }

        public async Task<int> GetNextId(string table, string field)
        {
            return await _dBContext.GetConnection().QuerySingleOrDefaultAsync<int>("select COALESCE(max(" + field + "),0)+1 as next_id from " + table, null, _dBContext.GetTransaction());
        }
    }
}
