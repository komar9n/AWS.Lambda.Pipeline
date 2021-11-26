using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AWS.Lambda.Pipeline.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.Lambda.Pipeline.Generic
{
    public sealed class GenericRequestPipeline<TCaller>
    {
        private readonly IServiceProvider _serviceProvider;
        private object _pipeline;
        private PipelineContext _requestContext;

        public GenericRequestPipeline(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public GenericRequestPipeline<TCaller> Build<TRequest, TResult>([CallerMemberName] string callerName = "")
        {
            if (callerName == null) throw new ArgumentNullException(nameof(callerName));

            var handlerAttributes = Utils.GetAttributes<GenericRequestPipelineAttribute>(typeof(TCaller), callerName);

            var handler = _serviceProvider.GetService<IPipelineHandler<TRequest, TResult>>();

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(IPipelineHandler<TRequest, TResult>));
            }

            var pipelineBuilder = new PipelineBuilder<TRequest, TResult>();

            _requestContext = new PipelineContext(typeof(TRequest), handlerAttributes);

            _pipeline = pipelineBuilder.BuildPipeline(handler, handlerAttributes);

            return this;
        }

        public async Task<PipelineResult<TResult>> Execute<TRequest, TResult>(TRequest request)
        {
            if (request == null) throw new ArgumentNullException("Request cannot be null");
            if (_pipeline == null) throw new ArgumentNullException("The Pipeline was not initialized");

            var pipeline = (IPipelineHandler<TRequest, TResult>)_pipeline;
            _requestContext.SetItem(Constants.RequestModelKey, request);

            return await pipeline.Handle(request, _requestContext);
        }
    }
}
