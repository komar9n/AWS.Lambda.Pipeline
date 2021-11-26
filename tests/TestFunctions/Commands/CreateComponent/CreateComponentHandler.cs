using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using AWS.Lambda.Pipeline;
using AWS.Lambda.Pipeline.APIGatewayProxy.Builders;
using TestFunctions.Models;

namespace TestFunctions.Commands.CreateComponent
{
    public class CreateComponentHandler : IPipelineHandler<CreateComponentCommand, APIGatewayProxyResponse>
    {
        public async Task<PipelineResult<APIGatewayProxyResponse>> Handle(CreateComponentCommand request, PipelineContext context)
        {
            var createdComponent = new Component { Id = 11, Description = request.Description, Name = request.Name };

            var response = new ApiGatewayProxyResponseBuilder<Component>()
                .JsonContent()
                .AllowCors()
                .StatusCode(HttpStatusCode.OK)
                .Body(createdComponent)
                .Build();

            return await Task.FromResult(PipelineResult<APIGatewayProxyResponse>.Success(response));
        }
    }
}
