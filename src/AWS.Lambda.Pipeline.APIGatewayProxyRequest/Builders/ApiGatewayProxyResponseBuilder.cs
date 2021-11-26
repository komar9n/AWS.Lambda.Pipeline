using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Builders
{
    public class ApiGatewayProxyResponseBuilder<T>
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly APIGatewayProxyResponse _response = new APIGatewayProxyResponse
        {
            Headers = new Dictionary<string, string>(),
        };

        public ApiGatewayProxyResponseBuilder<T> StatusCode(HttpStatusCode statusCode)
        {
            _response.StatusCode = (int)statusCode;

            return this;
        }

        public ApiGatewayProxyResponseBuilder<T> IsBase64Encoded(bool state)
        {
            _response.IsBase64Encoded = state;

            return this;
        }

        public ApiGatewayProxyResponseBuilder<T> Body(T payload)
        {
            _response.Body = JsonSerializer.Serialize(payload, _options);

            return this;
        }

        public ApiGatewayProxyResponseBuilder<T> AddHeader(string key, string value)
        {
            _response.Headers.Add(key, value);

            return this;
        }

        public ApiGatewayProxyResponseBuilder<T> JsonContent()
        {
            _response.Headers.Add("Content-Type", "text/json");

            return this;
        }

        public ApiGatewayProxyResponseBuilder<T> AllowCors()
        {
            _response.Headers.Add("Access-Control-Allow-Origin", "*");

            return this;
        }

        public APIGatewayProxyResponse Build() => _response;
    }
}
