using System.Threading.Tasks;

namespace AWS.Lambda.Pipeline
{
    public interface IPipelineHandler<in TRequest, TResponse>
    {
        Task<PipelineResult<TResponse>> Handle(TRequest request, PipelineContext context);
    }
}
