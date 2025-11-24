using System.Threading;
using System.Threading.Tasks;

namespace qsLibPack.UseCases.Abstractions
{
    /// <summary>
    /// Orquestrador para envio de requisições de Use Cases através de behaviors e handlers.
    /// </summary>
    public interface IUseCaseDispatcher
    {
        /// <summary>
        /// Envia uma requisição de Use Case.
        /// </summary>
        /// <typeparam name="TResponse">Tipo da resposta.</typeparam>
        /// <param name="request">Instância da requisição.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Resposta do use case.</returns>
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}