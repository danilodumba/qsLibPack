using System.Threading;
using System.Threading.Tasks;

namespace qsLibPack.UseCases.Abstractions
{
    /// <summary>
    /// Representa uma requisição de Use Case que produz uma resposta tipada.
    /// </summary>
    /// <typeparam name="TResponse">Tipo da resposta retornada pelo handler.</typeparam>
    public interface IRequest<TResponse> { }

    /// <summary>
    /// Marcador para comandos (operações que alteram estado).
    /// </summary>
    /// <typeparam name="TResponse">Tipo da resposta retornada pelo handler.</typeparam>
    public interface ICommand<TResponse> : IRequest<TResponse> { }

    /// <summary>
    /// Marcador para queries (operações somente leitura).
    /// </summary>
    /// <typeparam name="TResponse">Tipo da resposta retornada pelo handler.</typeparam>
    public interface IQuery<TResponse> : IRequest<TResponse> { }
}