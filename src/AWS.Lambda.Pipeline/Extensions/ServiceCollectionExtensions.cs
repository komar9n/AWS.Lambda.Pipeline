using System;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.Lambda.Pipeline.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Will scan all implementations of IPipelineHandler
        /// </summary>
        /// <param name="services">IServiceCollection's instance</param>
        /// <param name="type">The type in which assembly that should be scanned.</param>
        /// <returns></returns>
        public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Type type)
        {
            services.Scan(scan =>
            {
                scan.FromAssembliesOf(type)
                    .AddClasses(classes => classes.AssignableTo(typeof(IPipelineHandler<,>))).AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            return services;
        }
    }
}
