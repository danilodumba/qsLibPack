using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using qsLibPack.Validations;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.Exceptions;

namespace qsLibPack.UseCases.Behaviors
{
    /// <summary>
    /// Captura exceções e transforma em <see cref="UseCaseException"/> com log centralizado.
    /// </summary>
    /// <typeparam name="TRequest">Tipo da requisição.</typeparam>
    /// <typeparam name="TResponse">Tipo da resposta.</typeparam>
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger;

        public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next)
        {
            try
            {
                return await next();
            }
            catch (UseCaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao processar {RequestType}", typeof(TRequest).Name);
                throw new UseCaseException("Erro inesperado no processamento do use case.", new[] { new ErrorValidation("Unexpected", ex.Message) }, ex);
            }
        }
    }
}