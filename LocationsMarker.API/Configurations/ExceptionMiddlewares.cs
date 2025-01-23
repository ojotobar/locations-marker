using API.Common.Response.Model.Exceptions;
using API.Common.Response.Model.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace LocationsMarker.API.Configurations
{
    public static class ExceptionMiddlewares
    {
        internal static void ConfigureExceptionHandler(this WebApplication app, ILogger<Program> logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            UnauthorizedException => StatusCodes.Status401Unauthorized,
                            ForbiddenException => StatusCodes.Status403Forbidden,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        var response = GetResponse(context.Response.StatusCode, contextFeature.Error.Message);
                        await context.Response.WriteAsync(response);
                    }
                });
            });
        }

        internal static string GetResponse(int code, string message)
        {
            return code switch
            {
                400 => JsonConvert.SerializeObject(new BadRequestResponse(message)),
                401 => JsonConvert.SerializeObject(new UnauthorizedResponse(message)),
                403 => JsonConvert.SerializeObject(new ForbiddenResponse(message)),
                404 => JsonConvert.SerializeObject(new NotFoundResponse(message)),
                _ => JsonConvert.SerializeObject(new InternalServerErrorResponse(message)),
            };
        }
    }
}
