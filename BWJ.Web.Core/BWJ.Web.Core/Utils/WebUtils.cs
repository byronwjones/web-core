using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BWJ.Web.Core.Utils
{
    public static class WebUtils
    {
        public static IActionResult GetResult(HttpStatusCode status) => new StatusCodeResult((int) status);

        public static IActionResult GetResult(HttpStatusCode status, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return GetResult(status);
            }

            return new ContentResult()
            {
                Content = message,
                StatusCode = (int)status
            };
        }
    }
}
