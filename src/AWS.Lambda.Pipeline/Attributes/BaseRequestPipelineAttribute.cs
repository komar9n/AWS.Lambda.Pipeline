using System;

namespace AWS.Lambda.Pipeline.Attributes
{
    public class BaseRequestPipelineAttribute : Attribute
    {
        public BaseRequestPipelineAttribute(Type handler)
        {
            HandlerType = handler;
        }

        private BaseRequestPipelineAttribute()
        {
        }

        public Type HandlerType { get; }
    }
}
