using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BWJ.Web.Core.Exceptions
{
    public sealed class ConflictException : HttpException
    {
        public ConflictException(bool logRequest = true) : base() => Init(logRequest);
        public ConflictException(string message, bool logRequest = true) : base(message) => Init(logRequest);

        public override IActionResult GetResult()
        {
            return new ConflictResult();
        }
        public override IActionResult GetResultWithMessage()
        {
            return new ConflictObjectResult(Message);
        }

        private void Init(bool logRequest)
        {
            StatusCode = HttpStatusCode.Conflict;
            LogRequest = logRequest;
        }
    }
}
