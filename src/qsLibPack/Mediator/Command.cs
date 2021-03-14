using System.Collections.Generic;
using FluentValidation.Results;
using MediatR;

namespace qsLibPack.Mediator
{
    public abstract class Command<TResult> : IRequest<TResult>
    {
        public IList<ValidationFailure> Errors {get; private set; }
        public abstract bool IsValid();
    }
}