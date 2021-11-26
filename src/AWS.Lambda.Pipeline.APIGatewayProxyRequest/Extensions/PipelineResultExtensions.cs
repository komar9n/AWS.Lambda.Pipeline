using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using AWS.Lambda.Pipeline.APIGatewayProxy.Builders;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Extensions
{
    public static class PipelineResultExtensions
    {
        public static APIGatewayProxyResponse ConvertToApiGatewayProxyResponse<T>(this PipelineResult<T> result)
        {
            if (result.IsSuccess)
            {
                return new ApiGatewayProxyResponseBuilder<T>()
                    .JsonContent()
                    .AllowCors()
                    .StatusCode(HttpStatusCode.OK)
                    .Body(result.Result)
                    .Build();
            }

            if (result.IsFailure && result.ErrorCode != null)
            {
                return new ApiGatewayProxyResponseBuilder<string>()
                    .JsonContent()
                    .AllowCors()
                    .StatusCode((HttpStatusCode)result.ErrorCode)
                    .Body(result.Error)
                    .Build();
            }

            return default;
        }
    }
}
