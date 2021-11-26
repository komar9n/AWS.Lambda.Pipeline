using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AWS.Lambda.Pipeline;
using TestFunctions.Models;

namespace TestFunctions.Queries.GetComponents
{
    public class GetComponentsHandler : IPipelineHandler<GetComponentsQuery, IList<Component>>
    {
        public async Task<PipelineResult<IList<Component>>> Handle(GetComponentsQuery request, PipelineContext context)
        {
            var components = request.Ids.Select(c => new Component
                {Id = c, Name = $"test.name.{c}", Description = $"test.description.{c}"}).ToList();

            return await Task.FromResult(PipelineResult<IList<Component>>.Success(components));
        }
    }
}
