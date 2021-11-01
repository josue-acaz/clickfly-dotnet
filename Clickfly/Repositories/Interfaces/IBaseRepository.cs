using System;
using System.Threading.Tasks;

namespace clickfly.Repositories
{
    public interface IBaseRepository
    {
        Task<int> GetNextId(string table, string field);
    }
}