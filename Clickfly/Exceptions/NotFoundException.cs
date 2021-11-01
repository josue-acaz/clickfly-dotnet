using System;
using System.Net;

namespace clickfly.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message)
        {
            
        }
    }
}