using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using clickfly.ViewModels;

namespace clickfly.Data
{
    public interface IDBAccess
    {
        Task<Type> QuerySingleAsync<Type>(QueryOptions options);
        Task<IEnumerable<Type>> QueryAsync<Type>(QueryOptions options);
    }
}
