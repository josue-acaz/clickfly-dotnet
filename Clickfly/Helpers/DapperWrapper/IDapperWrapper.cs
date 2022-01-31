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

// TABELAS
/*

t
t0
t1
t2
t3
...

*/

// CAMPOS
/*

c
c0
c1
c2
c3

*/