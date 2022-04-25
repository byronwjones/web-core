using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BWJ.Web.Core.Exceptions
{
    public sealed class InsufficientPrivilegeException : HttpException
    {
        public InsufficientPrivilegeException(bool logRequest = true) : base() => Init(logRequest);
        public InsufficientPrivilegeException(string message, bool logRequest = true) : base(message) => Init(logRequest);

        public override IActionResult GetResult()
        {
            return new BadRequestResult();
        }
        public override IActionResult GetResultWithMessage()
        {
            return new BadRequestObjectResult(Message);
        }

        private void Init(bool logRequest)
        {
            StatusCode = HttpStatusCode.BadRequest;
            LogRequest = logRequest;
        }
    }
}
