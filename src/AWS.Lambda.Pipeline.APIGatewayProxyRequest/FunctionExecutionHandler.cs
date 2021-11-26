using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;

namespace AWS.Lambda.Pipeline.APIGatewayProxy
{
    public class FunctionExecutionHandler<TRequest> : IPipelineHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        private readonly IPipelineHandler<TRequest, APIGatewayProxyResponse> _next;

        public FunctionExecutionHandler(IPipelineHandler<TRequest, APIGatewayProxyResponse> next)
        {
            _next = next;
        }

        public async Task<PipelineResult<APIGatewayProxyResponse>> Handle(APIGatewayProxyRequest request, PipelineContext context)
        {
            return await _next.Handle(context.GetItem<TRequest>(Constants.RequestModelKey), context);
        }
    }
}
