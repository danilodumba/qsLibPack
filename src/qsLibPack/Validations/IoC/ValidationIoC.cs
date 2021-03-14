using Microsoft.Extensions.DependencyInjection;
using qsLibPack.Validations.Interface;

namespace qsLibPack.Validations.IoC
{
    public static class ValidationIoC
    {
        public static void AddValidationService(this IServiceCollection services)
        {
            services.AddScoped<IValidationService, ValidationService>();
        }
    }
}