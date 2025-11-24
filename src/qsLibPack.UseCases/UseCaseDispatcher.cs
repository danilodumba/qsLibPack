using System;
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

        public UseCaseDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var handlerType = typeof(IUseCaseHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            var handler = serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException($"Handler não encontrado para {requestType.Name}");

            var method = handlerType.GetMethod("Handle")!;

            var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
            var behaviors = serviceProvider.GetServices(pipelineType).Reverse().ToArray();

            Func<Task<TResponse>> last = () => (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken })!;

            foreach (var behavior in behaviors)
            {
                var handleMethod = pipelineType.GetMethod("Handle")!;
                var next = last;
                last = () => (Task<TResponse>)handleMethod.Invoke(behavior, new object[] { request, cancellationToken, next })!;
            }

            return last();
        }
    }
}