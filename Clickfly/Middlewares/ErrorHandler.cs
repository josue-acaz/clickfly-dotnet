using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using clickfly.ViewModels;

namespace clickfly.Middlewares
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _logger;
        private readonly ErrorHandlerOptions _options;

        public ErrorHandler(RequestDelegate next, ILoggerFactory logger, ErrorHandlerOptions options)
        {
            _next = next;
            _logger = logger;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.CreateLogger($"Unexpected error: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string Message = exception.Message == "" ? "An error occurred whilst processing your request" : exception.Message;

            object json = new
            {
                context.Response.StatusCode,
                Message = Message,
                StackTrace = exception
            };
            
            return context.Response.WriteAsync(JsonConvert.SerializeObject(json));
        }
    }
}
