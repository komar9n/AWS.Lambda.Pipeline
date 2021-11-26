using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.APIGatewayEvents;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Helpers
{
    public sealed class ApiGatewayProxyRequestGuard
    {
        private const string ErrorMessagesSeparator = ",";
        private const string ParameterNotFound = "Parameter '{0}' is not found";
        private const string BodyIsInvalid = "Can not deserialize object from body";

        private readonly APIGatewayProxyRequest _request;
        private readonly List<string> _errors = new List<string>();

        public ApiGatewayProxyRequestGuard(APIGatewayProxyRequest request)
        {
            _request = request;
        }

        public ApiGatewayProxyRequestGuard ShouldHaveQueryParameter(string parameterName)
        {
            if (parameterName is null)
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (_request.QueryStringParameters is null || !_request.QueryStringParameters.ContainsKey(parameterName))
            {
                _errors.Add(string.Format(ParameterNotFound, parameterName));
            }

            return this;
        }

        public ApiGatewayProxyRequestGuard ShouldHavePathParameter(string parameterName)
        {
            if (parameterName is null)
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (_request.PathParameters is null || !_request.PathParameters.ContainsKey(parameterName))
            {
                _errors.Add(string.Format(ParameterNotFound, parameterName));
            }

            return this;
        }

        public ApiGatewayProxyRequestGuard ShouldHaveBody()
        {
            if (_request.Body == null)
            {
                _errors.Add(BodyIsInvalid);
            }

            return this;
        }

        public Result Check()
        {
            return _errors.Any()
                ? Result.Fail(string.Join(ErrorMessagesSeparator, _errors))
                : Result.Success();
        }
    }
}
