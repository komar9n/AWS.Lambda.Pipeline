using FluentValidation;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.ProxyFluentValidation
{
    public class ProxyFluentValidationHandler<TRequest, TResult> : IPipelineHandler<TRequest, TResult>
    {
        private readonly IPipelineHandler<TRequest, TResult> _next;

        public ProxyFluentValidationHandler(IPipelineHandler<TRequest, TResult> next)
        {
            _next = next;
        }

        public async Task<PipelineResult<TResult>> Handle(TRequest request, PipelineContext context)
        {
            var validatorAttribute = context.RequestAttributes.First(a => a is ProxyFluentValidationAttribute) as ProxyFluentValidationAttribute;
            var validatorInstance = (IValidator)Activator.CreateInstance(validatorAttribute.ValidatorType);

            try
            {
                var validationContext = new ValidationContext<object>(context.GetItem<object>(Constants.RequestModelKey));
                var validationResult = await validatorInstance.ValidateAsync(validationContext);

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
