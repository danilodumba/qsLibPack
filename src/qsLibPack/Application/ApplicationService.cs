using qsLibPack.Repositories.Interfaces;
using qsLibPack.Validations.Interface;

namespace qsLibPack.Application
{
    public abstract class ApplicationService
    {
        protected readonly IValidationService _validationService;
        protected readonly IUnitOfWork _uow;

        protected ApplicationService(IValidationService validationService, IUnitOfWork uow)
        {
            _validationService = validationService;
            _uow = uow;
        }
    }
}