using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly
{
    public interface IOrm
    {
        Task<IEnumerable<Type>> QueryAsync<Type>(QueryAsyncParams queryAsyncParams) where Type : new();
    }
}