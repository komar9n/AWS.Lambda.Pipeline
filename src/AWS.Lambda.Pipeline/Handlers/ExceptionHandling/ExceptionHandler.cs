using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace AWS.Lambda.Pipeline.Handlers.ExceptionHandling
{
    internal class ExceptionHandler<TRequest, TResult> : IPipelineHandler<TRequest, TResult>
    {
        private readonly IPipelineHandler<TRequest, TResult> _next;
        private readonly bool _logStackTrace;

        public ExceptionHandler(IPipelineHandler<TRequest, TResult> next, bool logStackTrace)
        {
            _next = next;
            _logStackTrace = logStackTrace;
        }

        public async Task<PipelineResult<TResult>> Handle(TRequest request, PipelineContext context)
        {
            try
            {
                return await _next.Handle(request, context);
            }
            catch (Exception ex)
            {
                if (_logStackTrace)
                {
                    LambdaLogger.Log(ex.StackTrace);
                }

                return PipelineResult<TResult>.Fail(ex.Message, 500);
            }
        }
    }
}
