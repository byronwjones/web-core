using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BWJ.Web.Core.ApiRequests
{
    public interface IApiHandler<THandlerContext, TResponse> : IApiHandler<TResponse>
        where THandlerContext : class, IApiHandlerContext<TResponse>
        where TResponse: class
    {
        Task AssertUserHasPermission(THandlerContext cxt);
        Task AssertRequestIsValid(THandlerContext cxt);
        Task<TResponse> PerformAction(THandlerContext cxt);
    }

    public interface IApiHandler<TResponse> where TResponse : class
    {
        Task<TResponse> HandleRequest(IApiHandlerContext<TResponse> handlerContext, ILogger logger);
    }
}