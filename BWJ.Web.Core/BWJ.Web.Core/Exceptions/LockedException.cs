using BWJ.Web.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BWJ.Web.Core.Exceptions
{
    public sealed class LockedException : HttpException
    {
        public LockedException(bool logRequest = true) : base() => Init(logRequest);
        public LockedException(string message, bool logRequest = true) : base(message) => Init(logRequest);

        public override IActionResult GetResult()
        {
            return WebUtils.GetResult(StatusCode);
        }
        public override IActionResult GetResultWithMessage()
        {
            return WebUtils.GetResult(StatusCode, Message);
        }

        private void Init(bool logRequest)
        {
            StatusCode = HttpStatusCode.Locked;
            LogRequest = logRequest;
        }
    }
}
