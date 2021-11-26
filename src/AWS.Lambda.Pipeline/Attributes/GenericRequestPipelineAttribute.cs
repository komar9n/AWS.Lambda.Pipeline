using System;

namespace AWS.Lambda.Pipeline.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GenericRequestPipelineAttribute : BaseRequestPipelineAttribute
    {
        public GenericRequestPipelineAttribute(Type handler)
            : base(handler) { }
    }
}
