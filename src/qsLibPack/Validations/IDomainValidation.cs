using System.Collections.Generic;
using FluentValidation.Results;

namespace qs.Domain.Core.Validations
{
    public interface IDomainValidation
    {
        void AddErrors(string key, string description);
        void AddErrors(IList<ValidationFailure> erros);
        bool IsValid();
        IList<DomainError> GetErrors();
    }
}