using System.Collections.Generic;
using qsLibPack.Validations;

namespace qsLibPack.UseCases.Abstractions
{
    /// <summary>
    /// Representa uma resposta padrão de Use Case, incluindo sucesso e erros.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Indica se a operação foi concluída com sucesso.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Lista de erros associados à operação.
        /// </summary>
        IReadOnlyCollection<ErrorValidation> Errors { get; }
    }
}