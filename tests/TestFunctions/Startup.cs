using System;
using Amazon.Lambda.Core;
using AWS.Lambda.Pipeline.APIGatewayProxy;
using AWS.Lambda.Pipeline.Extensions;
using AWS.Lambda.Pipeline.Generic;
using Microsoft.Extensions.DependencyInjection;
using TestFunctions.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TestFunctions
{
    public class Startup
    {
        private static readonly Lazy<Startup> Lazy = new Lazy<Startup>(() => new Startup());

        public Startup()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        public static Startup Instance => Lazy.Value;

        public IServiceProvider ServiceProvider { get; }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHandlersFromAssembly(typeof(Entity));
            services.AddScoped(typeof(ApiGatewayProxyRequestPipeline<>));
            services.AddScoped(typeof(GenericRequestPipeline<>));
        }
    }
}
