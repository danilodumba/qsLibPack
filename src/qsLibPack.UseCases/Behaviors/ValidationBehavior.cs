using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
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

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators?.ToArray() ?? Array.Empty<IValidator<TRequest>>();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next)
        {
            if (validators.Length == 0)
                return await next().ConfigureAwait(false);

            var context = new ValidationContext<TRequest>(request);

            List<ValidationFailure> failures;
            if (validators.Length == 1)
            {
                var result = await validators[0].ValidateAsync(context, cancellationToken).ConfigureAwait(false);
                failures = result.Errors;
            }
            else
            {
                var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);
                failures = results.SelectMany(r => r.Errors).ToList();
            }

            if (failures.Count > 0)
            {
                var errors = failures.Where(f => f != null).Select(f => new ErrorValidation(f.ErrorCode ?? f.PropertyName, f.ErrorMessage)).ToArray();
                throw new UseCaseException("Validação falhou.", errors);
            }

            return await next().ConfigureAwait(false);
        }
    }
}
