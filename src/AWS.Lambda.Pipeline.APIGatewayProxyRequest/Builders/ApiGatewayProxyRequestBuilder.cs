using System.Collections.Generic;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Builders
{
    public class ApiGatewayProxyRequestBuilder
    {
        private readonly APIGatewayProxyRequest _request = new APIGatewayProxyRequest()
        {
            PathParameters = new Dictionary<string, string>(),
            QueryStringParameters = new Dictionary<string, string>(),
        };

        public ApiGatewayProxyRequestBuilder WithUser<TUser>(TUser user, string key = "user")
        {
            _request.RequestContext = new APIGatewayProxyRequest.ProxyRequestContext
            {
                Authorizer = new APIGatewayCustomAuthorizerContext
                {
                    { key, JsonSerializer.Serialize(user) },
                },
            };

            return this;
        }

        public ApiGatewayProxyRequestBuilder WithBody<T>(T requestPayload)
        {
            _request.Body = JsonSerializer.Serialize(requestPayload);

            return this;
        }

        public ApiGatewayProxyRequestBuilder WithPathParameter(string parameter, string parameterValue)
        {
            _request.PathParameters.Add(parameter, parameterValue);

            return this;
        }

        public ApiGatewayProxyRequestBuilder WithQueryStringParameter(string parameter, string parameterValue)
        {
            _request.QueryStringParameters.Add(parameter, parameterValue);

            return this;
        }

        public APIGatewayProxyRequest Build()
        {
            return _request;
        }
    }
}
