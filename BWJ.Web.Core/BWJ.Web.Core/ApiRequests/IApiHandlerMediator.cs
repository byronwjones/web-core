using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BWJ.Web.Core.ApiRequests
{
    public interface IApiHandlerMediator
    {
        Task<TResponse> Send<TResponse>(IApiHandlerContext<TResponse> context, ILogger logger) where TResponse : class;
    }
}