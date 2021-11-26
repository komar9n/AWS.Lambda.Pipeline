using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using AWS.Lambda.Pipeline;
using AWS.Lambda.Pipeline.APIGatewayProxy.Builders;
using TestFunctions.Models;

namespace TestFunctions.Commands.UpdateComponent
{
    public class UpdateComponentHandler : IPipelineHandler<UpdateComponentCommand, APIGatewayProxyResponse>
    {
        public async Task<PipelineResult<APIGatewayProxyResponse>> Handle(UpdateComponentCommand request, PipelineContext context)
        {
            var createdComponent = new Component { Id = request.Id, Description = request.Description, Name = request.Name };

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
