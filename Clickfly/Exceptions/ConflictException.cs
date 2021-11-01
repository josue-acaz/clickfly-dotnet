using System;
using System.Net;

namespace clickfly.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message) : base(HttpStatusCode.Conflict, message)
        {
            
        }
    }
}