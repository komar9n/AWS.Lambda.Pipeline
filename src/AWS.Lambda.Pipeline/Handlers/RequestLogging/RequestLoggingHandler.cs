using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace AWS.Lambda.Pipeline.Handlers.RequestLogging
{
    public class RequestLoggingHandler<TRequest, TResult> : IPipelineHandler<TRequest, TResult>
    {
        private readonly IPipelineHandler<TRequest, TResult> _next;

        public RequestLoggingHandler(IPipelineHandler<TRequest, TResult> next)
        {
            _next = next;
        }

        public async Task<PipelineResult<TResult>> Handle(TRequest request, PipelineContext context)
        {
            LambdaLogger.Log($"The request data: {JsonSerializer.Serialize(request)}");

            return await _next.Handle(request, context);
        }
    }
}
