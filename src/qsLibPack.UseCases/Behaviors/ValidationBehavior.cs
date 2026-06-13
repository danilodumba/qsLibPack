using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using qsLibPack.Validations;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.Exceptions;

namespace qsLibPack.UseCases.Behaviors
{
    /// <summary>
    /// Executa validações FluentValidation antes do handler.
    /// </summary>
    /// <typeparam name="TRequest">Tipo da requisição.</typeparam>
    /// <typeparam name="TResponse">Tipo da resposta.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest>[] validators;

        public ValidationBehavior(System.Collections.Generic.IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators?.ToArray() ?? System.Array.Empty<IValidator<TRequest>>();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, System.Func<Task<TResponse>> next)
        {
            if (validators.Length == 0)
                return await next().ConfigureAwait(false);

            var context = new ValidationContext<TRequest>(request);
            var failures = (await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count > 0)
            {
                var errors = failures.Select(f => new ErrorValidation(f.ErrorCode ?? f.PropertyName, f.ErrorMessage)).ToArray();
                throw new UseCaseException("Validação falhou.", errors);
            }

            return await next().ConfigureAwait(false);
        }
    }
}
