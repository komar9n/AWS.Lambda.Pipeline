using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using AWS.Lambda.Pipeline.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.Lambda.Pipeline.APIGatewayProxy
{
    public sealed class ApiGatewayProxyRequestPipeline<TCaller>
    {
        private readonly IServiceProvider _serviceProvider;
        private IPipelineHandler<APIGatewayProxyRequest, APIGatewayProxyResponse> _pipeline;
        private PipelineContext _requestContext;
        private APIGatewayProxyRequest _request;

        public ApiGatewayProxyRequestPipeline(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ApiGatewayProxyRequestPipeline<TCaller> Build<TRequest>(APIGatewayProxyRequest request, [CallerMemberName] string callerName = "")
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
            if (callerName == null) throw new ArgumentNullException(nameof(callerName));

            var handlerAttributes = Utils.GetAttributes<APIGatewayProxyRequestPipelineAttribute>(typeof(TCaller), callerName);

            var handler = _serviceProvider.GetService<IPipelineHandler<TRequest, APIGatewayProxyResponse>>();

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(IPipelineHandler<TRequest, APIGatewayProxyResponse>));
            }

            var pipelineBuilder = new PipelineBuilder<APIGatewayProxyRequest, APIGatewayProxyResponse>();

            Type functionExecutorType = typeof(FunctionExecutionHandler<>);
            Type[] typeArgs = { typeof(TRequest) };
            Type constructed = functionExecutorType.MakeGenericType(typeArgs);

            var requestDecorator = (IPipelineHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>)Activator.CreateInstance(constructed, handler);

            _requestContext = new PipelineContext(typeof(TRequest), handlerAttributes);

            _pipeline = pipelineBuilder.BuildPipeline(requestDecorator, handlerAttributes);

            return this;
        }

        public async Task<PipelineResult<APIGatewayProxyResponse>> Execute()
        {
            if (_request == null) throw new ArgumentNullException("Request cannot be null");
            if (_pipeline == null) throw new ArgumentNullException("The Pipeline was not initialized");

            return await _pipeline.Handle(_request, _requestContext);
        }
    }
}
