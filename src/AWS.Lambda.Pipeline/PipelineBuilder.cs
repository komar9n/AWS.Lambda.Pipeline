using System;
using System.Collections.Generic;
using System.Linq;
using AWS.Lambda.Pipeline.Attributes;
using AWS.Lambda.Pipeline.Handlers.ExceptionHandling;

namespace AWS.Lambda.Pipeline
{
    public class PipelineBuilder<TRequest, TResponse>
    {
        public IPipelineHandler<TRequest, TResponse> BuildPipeline(IPipelineHandler<TRequest, TResponse> handler, IList<BaseRequestPipelineAttribute> attributes)
        {
            foreach (var handlerAttribute in attributes.Reverse())
            {
                Type[] typeArgs = { typeof(TRequest), typeof(TResponse) };
                Type constructed = handlerAttribute.HandlerType.MakeGenericType(typeArgs);

                if (!(handlerAttribute is ExceptionHandlingAttribute))
                {
                    handler = (IPipelineHandler<TRequest, TResponse>)Activator.CreateInstance(constructed, handler);
                }
            }

            return AddExceptionHandling(handler, attributes);
        }

        private IPipelineHandler<TRequest, TResponse> AddExceptionHandling(IPipelineHandler<TRequest, TResponse> handler, IList<BaseRequestPipelineAttribute> attributes)
        {
            var exceptionHandlingAttribute = attributes.FirstOrDefault(a => a is ExceptionHandlingAttribute);

            if (exceptionHandlingAttribute == null || ((ExceptionHandlingAttribute)exceptionHandlingAttribute).Enabled)
            {
                handler = new ExceptionHandler<TRequest, TResponse>(handler, ((ExceptionHandlingAttribute)exceptionHandlingAttribute)?.LogStackTrace ?? true);
            }

            return handler;
        }
    }
}
