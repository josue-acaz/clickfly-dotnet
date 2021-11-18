using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using clickfly.ViewModels;

namespace clickfly.Data
{
    public interface IDBAccess
    {
        Task<T> QuerySingleAsync<T>(QueryOptions options);
        Task<IEnumerable<T>> QueryAsync<T>(QueryOptions options);
    }
}
