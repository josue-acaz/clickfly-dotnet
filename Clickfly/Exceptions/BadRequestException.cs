using System;
using System.Net;

namespace clickfly.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(HttpStatusCode.Conflict, message)
        {
            
        }
    }
}