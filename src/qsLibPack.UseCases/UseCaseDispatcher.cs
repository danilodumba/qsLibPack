using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.UseCases.Abstractions;

namespace qsLibPack.UseCases
{
    /// <summary>
    /// Implementação padrão do dispatcher de Use Cases.
    /// </summary>
    public class UseCaseDispatcher : IUseCaseDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        private static readonly ConcurrentDictionary<(Type RequestType, Type ResponseType), object> _wrappers
            = new ConcurrentDictionary<(Type, Type), object>();

        public UseCaseDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var wrapper = (HandlerWrapper<TResponse>)_wrappers.GetOrAdd(
                (request.GetType(), typeof(TResponse)),
                static key =>
                {
                    var wrapperType = typeof(HandlerWrapper<,>).MakeGenericType(key.RequestType, key.ResponseType);
                    return Activator.CreateInstance(wrapperType)!;
                });

            return wrapper.Handle(request, serviceProvider, cancellationToken);
        }

        private abstract class HandlerWrapper<TResponse>
        {
            public abstract Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
        }

        private sealed class HandlerWrapper<TRequest, TResponse> : HandlerWrapper<TResponse>
            where TRequest : IRequest<TResponse>
        {
            public override Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
            {
                var typedRequest = (TRequest)request;

                var handler = serviceProvider.GetService<IUseCaseHandler<TRequest, TResponse>>()
                    ?? throw new InvalidOperationException($"Handler não encontrado para {typeof(TRequest).Name}");

                var behaviorsEnum = serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>();
                var behaviors = behaviorsEnum as IPipelineBehavior<TRequest, TResponse>[] ?? behaviorsEnum.ToArray();

                Func<Task<TResponse>> next = () => handler.Handle(typedRequest, cancellationToken);

                for (var i = behaviors.Length - 1; i >= 0; i--)
                {
                    var behavior = behaviors[i];
                    var prev = next;
                    next = () => behavior.Handle(typedRequest, cancellationToken, prev);
                }

                return next();
            }
        }
    }
}
