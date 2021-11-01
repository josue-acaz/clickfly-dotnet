using System;
using System.Net;

namespace clickfly.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, message)
        {
            
        }
    }
}