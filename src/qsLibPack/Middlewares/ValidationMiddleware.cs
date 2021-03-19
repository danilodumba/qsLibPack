using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using qsLibPack.Validations.Interface;

namespace qsLibPack.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IValidationService validationService)
        {
            await _next(httpContext);

            if (!validationService.IsValid())
            {
                await this.HandleValidation(httpContext, validationService);
            }
        }

        private async Task HandleValidation(HttpContext context, IValidationService validationService)
        {
            var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

            if (!context.Response.HasStarted)
            {
                var result = JsonConvert.SerializeObject(validationService.GetErrors(), settings);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(result);                
            }
            else if (context.Response.StatusCode == 400)
            {
                context.Response.Clear();
                var result = JsonConvert.SerializeObject(validationService.GetErrors(), settings);
                await context.Response.WriteAsync(result);             
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