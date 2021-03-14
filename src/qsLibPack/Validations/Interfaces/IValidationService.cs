using System.Collections.Generic;
using FluentValidation.Results;

namespace qsLibPack.Validations.Interface
{
    public interface IValidationService
    {
        void AddErrors(string key, string description);
        void AddErrors(IList<ValidationFailure> erros);
        bool IsValid();
        IList<ErrorValidation> GetErrors();
    }
}