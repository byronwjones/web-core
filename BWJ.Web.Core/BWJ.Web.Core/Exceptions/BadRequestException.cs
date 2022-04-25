using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BWJ.Web.Core.Exceptions
{
    public sealed class BadRequestException : HttpException
    {
        public BadRequestException(bool logRequest = true) : base() => Init(logRequest);
        public BadRequestException(string message, bool logRequest = true) : base(message) => Init(logRequest);

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
