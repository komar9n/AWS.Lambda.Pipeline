using System;
using AWS.Lambda.Pipeline.Attributes;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.Warming
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class WarmerAttribute : APIGatewayProxyRequestPipelineAttribute
    {
        public WarmerAttribute()
            : base(typeof(WarmerHandler<,>))
        {
        }
    }
}
