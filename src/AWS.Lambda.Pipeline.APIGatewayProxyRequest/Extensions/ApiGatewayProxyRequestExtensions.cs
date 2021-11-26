using System;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Extensions
{
    internal static class ApiGatewayProxyRequestExtensions
    {
        public static T GetPathParameter<T>(this APIGatewayProxyRequest request, string parameterName)
        {
            var value = string.Empty;

            if (request.PathParameters?.TryGetValue(parameterName, out value) == true && !string.IsNullOrWhiteSpace(value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return default;
        }

        public static T GetQueryStringParameter<T>(this APIGatewayProxyRequest request, string parameterName)
        {
            var value = string.Empty;

            if (request.QueryStringParameters?.TryGetValue(parameterName, out value) == true && !string.IsNullOrWhiteSpace(value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return default;
        }

        public static T FromBody<T>(this APIGatewayProxyRequest request)
        {
            try
            {
                var deserializedObject = JsonSerializer.Deserialize<T>(request.Body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return deserializedObject;
            }
            catch
            {
                return default;
            }
        }
    }
}
