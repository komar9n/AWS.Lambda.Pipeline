using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace AWS.Lambda.Pipeline.Generic.Handlers.FluentValidation
{
    public class FluentValidationHandler<TRequest, TResult> : IPipelineHandler<TRequest, TResult>
    {
        private readonly IPipelineHandler<TRequest, TResult> _next;

        public FluentValidationHandler(IPipelineHandler<TRequest, TResult> next)
        {
            _next = next;
        }

        public async Task<PipelineResult<TResult>> Handle(TRequest request, PipelineContext context)
        {
            var validatorAttribute = context.RequestAttributes.First(a => a is FluentValidationAttribute) as FluentValidationAttribute;
            var validatorInstance = (IValidator<TRequest>)Activator.CreateInstance(validatorAttribute.ValidatorType);

            try
            {
                var validationResult = await validatorInstance.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var responseBuilder = new StringBuilder();
                    validationResult.Errors.ForEach(c => responseBuilder.AppendJoin(";", c.ErrorMessage));

                    return PipelineResult<TResult>.Fail(responseBuilder.ToString(), (int)HttpStatusCode.BadRequest);
                }
            }
            catch (ValidationException ex)
            {
                var responseBuilder = new StringBuilder();
                ex.Errors.ToList().ForEach(c => responseBuilder.AppendJoin(";", c.ErrorMessage));

                return PipelineResult<TResult>.Fail(responseBuilder.ToString(), (int)HttpStatusCode.BadRequest);
            }

            if (_next != null)
            {
                return await _next.Handle(request, context);
            }

            return null;
        }
    }
}
