using System.Threading;
using System.Threading.Tasks;

namespace qsLibPack.UseCases.Abstractions
{
    /// <summary>
    /// Contrato para handlers de Use Cases.
    /// </summary>
    /// <typeparam name="TRequest">Tipo da requisição.</typeparam>
    /// <typeparam name="TResponse">Tipo da resposta.</typeparam>
    public interface IUseCaseHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Executa o processamento do Use Case.
        /// </summary>
        /// <param name="request">Requisição do use case.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Resposta do use case.</returns>
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}