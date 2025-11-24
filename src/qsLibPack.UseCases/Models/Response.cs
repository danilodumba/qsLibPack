using System.Collections.Generic;
using System.Collections.ObjectModel;
using qsLibPack.Validations;
using qsLibPack.UseCases.Abstractions;

namespace qsLibPack.UseCases.Models
{
    /// <summary>
    /// Resposta padrão de Use Case.
    /// </summary>
    public sealed class Response : IResponse
    {
        public bool Success { get; }
        public IReadOnlyCollection<ErrorValidation> Errors { get; }

        private Response(bool success, IReadOnlyCollection<ErrorValidation> errors)
        {
            Success = success;
            Errors = errors;
        }

        /// <summary>
        /// Cria resposta de sucesso.
        /// </summary>
        public static Response Ok() => new Response(true, new ReadOnlyCollection<ErrorValidation>(new List<ErrorValidation>()));

        /// <summary>
        /// Cria resposta de falha.
        /// </summary>
        public static Response Fail(IEnumerable<ErrorValidation> errors) => new Response(false, new ReadOnlyCollection<ErrorValidation>(new List<ErrorValidation>(errors)));
    }

    /// <summary>
    /// Resposta padrão com payload.
    /// </summary>
    /// <typeparam name="T">Tipo do payload.</typeparam>
    public sealed class Response<T> : IResponse
    {
        public bool Success { get; }
        public IReadOnlyCollection<ErrorValidation> Errors { get; }
        public T? Data { get; }

        private Response(bool success, T? data, IReadOnlyCollection<ErrorValidation> errors)
        {
            Success = success;
            Data = data;
            Errors = errors;
        }

        /// <summary>
        /// Cria resposta de sucesso com payload.
        /// </summary>
        public static Response<T> Ok(T data) => new Response<T>(true, data, new ReadOnlyCollection<ErrorValidation>(new List<ErrorValidation>()));

        /// <summary>
        /// Cria resposta de falha.
        /// </summary>
        public static Response<T> Fail(IEnumerable<ErrorValidation> errors) => new Response<T>(false, default, new ReadOnlyCollection<ErrorValidation>(new List<ErrorValidation>(errors)));
    }
}