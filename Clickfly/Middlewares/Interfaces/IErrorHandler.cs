using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace clickfly.Middlewares
{
    public interface IErrorHandler
    {
        Task Invoke(HttpContext context);
    }
}