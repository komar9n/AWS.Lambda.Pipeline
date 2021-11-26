using System;
using AWS.Lambda.Pipeline.Attributes;

namespace AWS.Lambda.Pipeline.Handlers.ExceptionHandling
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExceptionHandlingAttribute : BaseRequestPipelineAttribute
    {
        public ExceptionHandlingAttribute()
            : base(typeof(ExceptionHandler<,>)) {}

        public bool Enabled { get; set; } = true;

        public bool LogStackTrace { get; set; } = true;
    }
}
