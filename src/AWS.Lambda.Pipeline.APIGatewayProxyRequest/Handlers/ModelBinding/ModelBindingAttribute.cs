using System;
using AWS.Lambda.Pipeline.Attributes;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.ModelBinding
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ModelBindingAttribute : APIGatewayProxyRequestPipelineAttribute
    {
        public ModelBindingAttribute()
            : base(typeof(ModelBindingHandler<,>))
        {
        }
    }
}
