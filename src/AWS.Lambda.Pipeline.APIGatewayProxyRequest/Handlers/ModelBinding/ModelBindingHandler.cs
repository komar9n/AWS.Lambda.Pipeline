using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using AWS.Lambda.Pipeline.APIGatewayProxy.Extensions;
using AWS.Lambda.Pipeline.APIGatewayProxy.Helpers;
using AWS.Lambda.Pipeline.Attributes.Binding;

namespace AWS.Lambda.Pipeline.APIGatewayProxy.Handlers.ModelBinding
{
    internal sealed class ModelBindingHandler<TRequest, TResult> : IPipelineHandler<TRequest, TResult>
    {
        private readonly IPipelineHandler<TRequest, TResult> _next;

        public ModelBindingHandler(IPipelineHandler<TRequest, TResult> next)
        {
            _next = next;
        }

        public async Task<PipelineResult<TResult>> Handle(TRequest request, PipelineContext context)
        {
            if (request is APIGatewayProxyRequest proxyRequest)
            {
                var guard = new ApiGatewayProxyRequestGuard(proxyRequest);

                var requestAttributes = context.RequestType.GetCustomAttributes(false);
                var requestProperties = context.RequestType.GetProperties();

                if (requestAttributes.Any(a => (a is FromBodyAttribute)))
                {
                    guard.ShouldHaveBody();
                }

                foreach (var requestProperty in requestProperties)
                {
                    var customAttributes = requestProperty.GetCustomAttributes(false);

                    foreach (var customAttribute in customAttributes)
                    {
                        switch (customAttribute)
                        {
                            case FromPathAttribute pathAttribute:
                                if (pathAttribute.IsRequired)
                                {
                                    guard.ShouldHavePathParameter(pathAttribute.PropertyName);
                                }

                                break;
                            case FromQueryAttribute queryAttribute:
                                if (queryAttribute.IsRequired)
                                {
                                    guard.ShouldHaveQueryParameter(queryAttribute.PropertyName);
                                }

                                break;
                            case FromBodyAttribute _:
                                guard.ShouldHaveBody();
                                break;
                        }
                    }
                }

                var requestValidationResult = guard.Check();

                if (requestValidationResult.IsFailure)
                {
                    return PipelineResult<TResult>.Fail(requestValidationResult.Error, (int)HttpStatusCode.BadRequest);
                }

                var method = typeof(ModelBindingHandler<TRequest,TResult>).GetMethod(nameof(BuildRequest));
                var generic = method.MakeGenericMethod(context.RequestType);
                var requestObject = generic.Invoke(this, new object[] {request, requestAttributes, requestProperties});

                context.SetItem(Constants.RequestModelKey, requestObject);

                if (_next != null)
                {
                    return await _next.Handle(request, context);
                }

                return null;
            }
            else
            {
                throw new ArgumentException("The request is not APIGatewayProxyRequest");
            }
        }

        public T BuildRequest<T>(APIGatewayProxyRequest request, object[] attributes, PropertyInfo[] properties)
        {
            T commandInstance = default;

            if (attributes.Any(a => (a is FromBodyAttribute)))
            {
                commandInstance = request.FromBody<T>();
            }
            else
            {
                commandInstance = (T)Activator.CreateInstance(typeof(T));
            }

            foreach (var property in properties)
            {
                var customAttributes = property.GetCustomAttributes(false);

                foreach (var customAttribute in customAttributes)
                {
                    switch (customAttribute)
                    {
                        case FromPathAttribute pathAttribute:
                            var pathAttributeValue = Convert.ChangeType(request.GetPathParameter<object>(pathAttribute.PropertyName), property.PropertyType);

                            property.SetValue(commandInstance, pathAttributeValue);
                            break;
                        case FromQueryAttribute queryAttribute:
                            var queryAttributeValue = Convert.ChangeType(request.GetQueryStringParameter<object>(queryAttribute.PropertyName), property.PropertyType);

                            property.SetValue(commandInstance, queryAttributeValue);
                            break;
                        case FromBodyAttribute _:
                            var value = Convert.ChangeType(request.FromBody<object>(), property.PropertyType);

                            property.SetValue(commandInstance, value);
                            break;
                    }
                }
            }

            return commandInstance;
        }
    }
}
