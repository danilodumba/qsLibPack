using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using qsLibPack.Validations;

namespace qsLibPack.UseCases.Exceptions
{
    /// <summary>
    /// Exceção padrão para falhas em Use Cases, contendo erros de validação.
    /// </summary>
    public class UseCaseException : Exception
    {
        /// <summary>
        /// Erros associados à falha.
        /// </summary>
        public IReadOnlyCollection<ErrorValidation> Errors { get; }

        public UseCaseException(string message, IEnumerable<ErrorValidation> errors, Exception? innerException = null) : base(message, innerException)
        {
            Errors = new ReadOnlyCollection<ErrorValidation>(new List<ErrorValidation>(errors));
        }
    }
}