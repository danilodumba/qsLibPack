using Microsoft.AspNetCore.Mvc;
using qsLibPack.Validations.Interface;

namespace qsLibPack.Controllers
{
    public abstract class ApiLibController: ControllerBase
    {
        readonly IValidationService _validationService;

        protected ApiLibController(IValidationService validationService)
        {
            _validationService = validationService;
        }

        protected IActionResult LibResult(IActionResult result)
        {
            if (_validationService.IsValid())
            {
                return result;
            }

            return BadRequest(_validationService.GetErrors());
        }
    }
}