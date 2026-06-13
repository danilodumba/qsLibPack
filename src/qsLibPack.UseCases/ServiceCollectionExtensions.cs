using System;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.UseCases;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.Behaviors;

namespace qsLibPack.UseCases.IoC
{
    /// <summary>
    /// Extensões para registro de Use Cases e behaviors via DI.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra dispatcher, handlers, behaviors e validators.
        /// </summary>
        /// <param name="services">Coleção de serviços.</param>
        /// <param name="assemblies">Assemblies para varredura de handlers/validators.</param>
        public static IServiceCollection AddUseCases(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddScoped<IUseCaseDispatcher, UseCaseDispatcher>();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract || type.IsInterface)
                        continue;

                    foreach (var iface in type.GetInterfaces())
                    {
                        if (!iface.IsGenericType)
                            continue;

                        var definition = iface.GetGenericTypeDefinition();
                        if (definition == typeof(IUseCaseHandler<,>) || definition == typeof(IValidator<>))
                        {
                            services.AddScoped(iface, type);
                        }
                    }
                }
            }

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            return services;
        }
    }
}
