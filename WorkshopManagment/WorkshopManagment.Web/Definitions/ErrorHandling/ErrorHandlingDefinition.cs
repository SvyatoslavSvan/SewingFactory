using Calabonga.AspNetCore.AppDefinitions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Security.Authentication;
using SewingFactory.Common.Domain.Exceptions;          

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.ErrorHandling;

public sealed class ErrorHandlingDefinition : AppDefinition
{
    public override bool Enabled => true;

    public override void ConfigureApplication(WebApplication app) =>
        app.UseExceptionHandler(error => error.Run(async context =>
        {
            var feature = context.Features.Get<IExceptionHandlerFeature>();
            if (feature is null)
            {
                return;
            }

            var ex = feature.Error;
            var status = (int)GetErrorCode(ex);

            Log.Error(ex, "Unhandled exception");

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";

            object payload = app.Environment.IsDevelopment()
                ? new
                {
                    status,
                    error = ex.GetType().Name,
                    message = ex.Message,
                    stack = ex.StackTrace
                }
                : new
                {
                    status,
                    message = status switch
                    {
                        400 => "The request contains invalid data.",
                        403 => "Access denied.",
                        404 => "Resource not found.",
                        501 => "Not implemented.",
                        _ => "Internal server error. Please try again later."
                    }
                };
            await context.Response.WriteAsJsonAsync(payload);
        }));

    private static HttpStatusCode GetErrorCode(Exception e) => e switch
    {
        ValidationException
            or SewingFactoryArgumentException
            or SewingFactoryArgumentNullException
            or SewingFactoryArgumentOutOfRangeException
            => HttpStatusCode.BadRequest,

        SewingFactoryAccessDeniedException
            or AuthenticationException
            => HttpStatusCode.Forbidden,

        SewingFactoryNotFoundException
            => HttpStatusCode.NotFound,

        NotImplementedException
            => HttpStatusCode.NotImplemented,

        _ => HttpStatusCode.InternalServerError
    };
}
