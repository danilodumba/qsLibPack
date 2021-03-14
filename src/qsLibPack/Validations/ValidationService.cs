using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using qsLibPack.Validations.Interface;

namespace qsLibPack.Validations
{
    public class ValidationService : IValidationService
    {
        readonly List<ErrorValidation> errors; 

        public ValidationService()
        {
            errors = new List<ErrorValidation>();
        }

        public void AddErrors(string key, string description)
        {
            this.errors.Add(new ErrorValidation(key, description));
        }

        public void AddErrors(IList<ValidationFailure> erros)
        {
            foreach(var error in erros)
            {
                this.AddErrors(error.ErrorCode, error.ErrorMessage);
            }
        }

        public IList<ErrorValidation> GetErrors()
        {
            return this.errors;
        }

        public bool IsValid()
        {
            return !this.errors.Any();
        }
    }
}