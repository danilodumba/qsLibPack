using System;
using System.Linq;
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
                var handlerTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(t => t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUseCaseHandler<,>))
                        .Select(i => new { Implementation = t, Service = i }))
                    .ToArray();

                foreach (var ht in handlerTypes)
                {
                    services.AddScoped(ht.Service, ht.Implementation);
                }

                var validatorTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(t => t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
                        .Select(i => new { Implementation = t, Service = i }))
                    .ToArray();

                foreach (var vt in validatorTypes)
                {
                    services.AddScoped(vt.Service, vt.Implementation);
                }
            }

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            return services;
        }
    }
}