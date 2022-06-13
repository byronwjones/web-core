using BWJ.Web.Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BWJ.Web.Core.ApiRequests
{
    public abstract class ApiHandler<THandlerContext, TResponse> : IApiHandler<THandlerContext, TResponse>
        where THandlerContext : class, IApiHandlerContext<TResponse>
        where TResponse : class
    {
        public abstract Task AssertUserHasPermission(THandlerContext cxt);

        public abstract Task AssertRequestIsValid(THandlerContext cxt);

        public abstract Task<TResponse> PerformAction(THandlerContext cxt);

        public async Task<TResponse> HandleRequest(IApiHandlerContext<TResponse> handlerContext, ILogger logger)
        {
            MethodGuard.NoNull(new { handlerContext, logger });
            MethodGuard.Acceptable<IApiHandlerContext<TResponse>>(
                new { handlerContext },
                r => r is THandlerContext,
                $"Argument must be of type {typeof(THandlerContext).FullName}");

            var context = handlerContext as THandlerContext;
            _log = logger;

            await PerformHandlerStage(context, AssertUserHasPermission, "Assert User Has Permission");
            await PerformHandlerStage(context, AssertRequestIsValid, "Assert Request Is Valid");

            var startTime = DateTime.UtcNow;
            var response = await PerformAction(context);

            _log.LogTrace("Handler stage {HandlerStage} completed in {ElaspedTime}ms",
                "Perform Action",
                (DateTime.UtcNow - startTime).TotalMilliseconds);

            return response;
        }

        private async Task PerformHandlerStage(THandlerContext handlerContext, Func<THandlerContext, Task> action, string actionName)
        {
            var startTime = DateTime.UtcNow;
            await action(handlerContext);

            _log.LogTrace("Handler stage {HandlerStage} completed in {ElaspedTime}ms",
                actionName,
                (DateTime.UtcNow - startTime).TotalMilliseconds);
        }

        protected ILogger _log = default;
    }
}
