using System;
using System.Threading.Tasks;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IBaseRepository
    {
        Task<int> GetNextId(string table, string field);
    }
}