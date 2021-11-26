using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.Warming
{
    public class WarmerHandler<TRequest, TResult> : IPipelineHandler<TRequest, TResult>
    {
        private readonly IPipelineHandler<TRequest, TResult> _next;

        public WarmerHandler(IPipelineHandler<TRequest, TResult> next)
        {
            _next = next;
        }

        public async Task<PipelineResult<TResult>> Handle(TRequest request, PipelineContext context)
        {
            if (request is APIGatewayProxyRequest proxyRequest)
            {
                if (proxyRequest.HttpMethod == null)
                {
                    return PipelineResult<TResult>.Fail("NoContent", (int)HttpStatusCode.NoContent);
                }
                else
                {
                    if (_next != null)
                    {
                        return await _next.Handle(request, context);
                    }

                    return null;
                }
            } 
            else
            {
                throw new ArgumentException("The request is not APIGatewayProxyRequest");
            }
        }
    }
}
