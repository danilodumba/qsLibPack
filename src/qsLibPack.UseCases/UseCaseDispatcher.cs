using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
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

        private static readonly ConcurrentDictionary<(Type, Type), (Type handlerType, MethodInfo handlerMethod, Type pipelineType, MethodInfo pipelineMethod)> _cache
            = new ConcurrentDictionary<(Type, Type), (Type, MethodInfo, Type, MethodInfo)>();

        public UseCaseDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            var (handlerType, handlerMethod, pipelineType, pipelineMethod) = _cache.GetOrAdd(
                (requestType, responseType),
                key =>
                {
                    var ht = typeof(IUseCaseHandler<,>).MakeGenericType(key.Item1, key.Item2);
                    var hm = ht.GetMethod("Handle")!;
                    var pt = typeof(IPipelineBehavior<,>).MakeGenericType(key.Item1, key.Item2);
                    var pm = pt.GetMethod("Handle")!;
                    return (ht, hm, pt, pm);
                });

            var handler = serviceProvider.GetService(handlerType)
                ?? throw new InvalidOperationException($"Handler não encontrado para {requestType.Name}");

            var behaviors = serviceProvider.GetServices(pipelineType).Reverse().ToArray();

            Func<Task<TResponse>> last = () => InvokeMethod<TResponse>(handlerMethod, handler, new object[] { request, cancellationToken });

            foreach (var behavior in behaviors)
            {
                var capturedBehavior = behavior;
                var next = last;
                last = () => InvokeMethod<TResponse>(pipelineMethod, capturedBehavior, new object[] { request, cancellationToken, next });
            }

            return last();
        }

        private static Task<TResponse> InvokeMethod<TResponse>(MethodInfo method, object target, object[] args)
        {
            try
            {
                return (Task<TResponse>)method.Invoke(target, args)!;
            }
            catch (TargetInvocationException tie) when (tie.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                throw;
            }
        }
    }
}
