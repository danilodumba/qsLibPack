using System.Collections.Generic;
using FluentValidation.Results;

namespace qsLibPack.Application
{
    public abstract class Input
    {
        public IList<ValidationFailure> Errors {get; private set; }
        public abstract bool IsValid();
    }
}