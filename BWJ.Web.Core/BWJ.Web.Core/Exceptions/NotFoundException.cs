using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BWJ.Web.Core.Exceptions
{
    public sealed class NotFoundException : HttpException
    {
        public NotFoundException(bool logRequest = true) : base() => Init(logRequest);
        public NotFoundException(string message, bool logRequest = true) : base(message) => Init(logRequest);

        public override IActionResult GetResult()
        {
            return new NotFoundResult();
        }
        public override IActionResult GetResultWithMessage()
        {
            return new NotFoundObjectResult(Message);
        }

        private void Init(bool logRequest)
        {
            StatusCode = HttpStatusCode.NotFound;
            LogRequest = logRequest;
        }
    }
}
