using AWS.Lambda.Pipeline.Attributes;

namespace AWS.Lambda.Pipeline.Handlers.RequestLogging
{
    public class RequestLoggingAttribute : BaseRequestPipelineAttribute
    {
        public RequestLoggingAttribute()
            : base(typeof(RequestLoggingHandler<,>)) { }
    }
}
