using System;
using AWS.Lambda.Pipeline.Attributes;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.ProxyFluentValidation
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ProxyFluentValidationAttribute : APIGatewayProxyRequestPipelineAttribute
    {
        public ProxyFluentValidationAttribute(Type validatorType)
            : base(typeof(ProxyFluentValidationHandler<,>))
        {
            ValidatorType = validatorType;
        }

        public Type ValidatorType { get; }
    }
}
