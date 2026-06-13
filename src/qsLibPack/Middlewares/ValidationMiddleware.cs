using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using qsLibPack.Validations.Interface;

namespace qsLibPack.Middlewares
{
    public class ValidationMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IValidationService validationService)
        {
            await _next(httpContext).ConfigureAwait(false);

            if (!validationService.IsValid())
            {
                await HandleValidation(httpContext, validationService).ConfigureAwait(false);
            }
        }

        private static async Task HandleValidation(HttpContext context, IValidationService validationService)
        {
            if (!context.Response.HasStarted)
            {
                var result = JsonSerializer.Serialize(validationService.GetErrors(), JsonOptions);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(result, context.RequestAborted).ConfigureAwait(false);
            }
        }
    }

    public static class ValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidationService(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidationMiddleware>();
        }
    }
}
