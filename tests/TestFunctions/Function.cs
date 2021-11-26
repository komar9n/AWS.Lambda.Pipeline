using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AWS.Lambda.Pipeline.APIGatewayProxy;
using AWS.Lambda.Pipeline.APIGatewayProxy.Extensions;
using AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.ModelBinding;
using AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.ProxyFluentValidation;
using AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.Warming;
using AWS.Lambda.Pipeline.Generic;
using AWS.Lambda.Pipeline.Generic.Handlers.FluentValidation;
using AWS.Lambda.Pipeline.Handlers.ExceptionHandling;
using AWS.Lambda.Pipeline.Handlers.RequestLogging;
using Microsoft.Extensions.DependencyInjection;
using TestFunctions.Commands.CreateComponent;
using TestFunctions.Commands.UpdateComponent;
using TestFunctions.Models;
using TestFunctions.Queries.GetComponents;

namespace TestFunctions
{
    public class Function
    {
        private readonly ApiGatewayProxyRequestPipeline<Function> _proxyPipeline;
        private readonly GenericRequestPipeline<Function> _genericPipeline;

        public Function()
        {
            _proxyPipeline = Startup.Instance.ServiceProvider.GetService<ApiGatewayProxyRequestPipeline<Function>>();
            _genericPipeline = Startup.Instance.ServiceProvider.GetService<GenericRequestPipeline<Function>>();
        }

        [RequestLogging]
        [FluentValidation(typeof(GetComponentsValidator))]
        public async Task<IList<Component>> GetComponents(IList<int> ids, ILambdaContext context)
        {
            var result = await _genericPipeline
                .Build<GetComponentsQuery, IList<Component>>()
                .Execute<GetComponentsQuery, IList<Component>>(new GetComponentsQuery { Ids = ids });

            return result.Result;
        }

        [ExceptionHandling]
        [Warmer]
        [ModelBinding]
        [ProxyFluentValidation(typeof(CreateComponentValidator))]
        public async Task<APIGatewayProxyResponse> CreateComponent(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var result = await _proxyPipeline
                .Build<CreateComponentCommand>(request)
                .Execute();

            return result.ConvertToApiGatewayProxyResponse();
        }

        [Warmer]
        [ModelBinding]
        [ProxyFluentValidation(typeof(UpdateComponentValidator))]
        public async Task<APIGatewayProxyResponse> UpdateComponent(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var result = await _proxyPipeline
                .Build<UpdateComponentCommand>(request)
                .Execute();

            return result.ConvertToApiGatewayProxyResponse();
        }
    }
}
