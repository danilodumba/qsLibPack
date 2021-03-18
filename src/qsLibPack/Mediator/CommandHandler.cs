using System.Threading;
using System.Threading.Tasks;
using MediatR;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.Validations.Interface;

namespace qsLibPack.Mediator
{
    public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : Command<TResponse>
    {
        protected readonly IValidationService _validationService;
        protected readonly IUnitOfWork _uow;

        protected CommandHandler(IValidationService validationService, IUnitOfWork uow)
        {
            _validationService = validationService;
            _uow = uow;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        protected bool CommandIsValid(TRequest request)
        {
            if (!request.IsValid())
            {
                _validationService.AddErrors(request.Errors);
                return false;
            }
            return true;
        }
    }
}