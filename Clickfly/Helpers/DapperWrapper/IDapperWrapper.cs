using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace clickfly.Data
{
    public interface IDapperWrapper
    {
        Task<IEnumerable<T>> QueryAsync<T>(SelectOptions options);
        Task<T> QuerySingleAsync<T>(SelectOptions options);
        Task InsertAsync<T>(InsertOptions options);
        Task UpdateAsync<T>(UpdateOptions options);
        int Count<T>(CountOptions options);
    }
}