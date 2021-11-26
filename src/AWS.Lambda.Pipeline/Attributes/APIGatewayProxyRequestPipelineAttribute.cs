using System;

namespace AWS.Lambda.Pipeline.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class APIGatewayProxyRequestPipelineAttribute : BaseRequestPipelineAttribute
    {
        public APIGatewayProxyRequestPipelineAttribute(Type handler)
            : base(handler) { }
    }
}
