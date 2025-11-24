using System;
using System.Threading;
using System.Threading.Tasks;

namespace qsLibPack.UseCases.Abstractions
{
    /// <summary>
    /// Comportamento de pipeline executado antes/depois do handler.
    /// </summary>
    /// <typeparam name="TRequest">Tipo da requisição.</typeparam>
    /// <typeparam name="TResponse">Tipo da resposta.</typeparam>
    public interface IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Executa o comportamento de pipeline.
        /// </summary>
        /// <param name="request">Requisição.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <param name="next">Delegado para execução do próximo estágio.</param>
        /// <returns>Resposta do use case.</returns>
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next);
    }
}