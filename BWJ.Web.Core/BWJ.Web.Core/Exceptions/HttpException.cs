using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace BWJ.Web.Core.Exceptions
{
    public abstract class HttpException : Exception
    {
        public HttpException() : base() { }
        public HttpException(string message) : base(message) { }

        public bool LogRequest { get; protected set; }
        public HttpStatusCode StatusCode { get; protected set; }

        public abstract IActionResult GetResult();
        public abstract IActionResult GetResultWithMessage();
    }
}
