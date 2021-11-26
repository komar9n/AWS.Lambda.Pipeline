using System;
using AWS.Lambda.Pipeline.Attributes;

namespace AWS.Lambda.Pipeline.Generic.Handlers.FluentValidation
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class FluentValidationAttribute : APIGatewayProxyRequestPipelineAttribute
    {
        public FluentValidationAttribute(Type validatorType)
            : base(typeof(FluentValidationHandler<,>))
        {
            ValidatorType = validatorType;
        }

        public Type ValidatorType { get; }
    }
}
