using Calabonga.AspNetCore.AppDefinitions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.IdentityServer.Web.Application.Features.Profile.Queries;
using SewingFactory.Backend.IdentityServer.Web.Application.Features.Profile.ViewModels;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.IdentityServer.Web.Endpoints;

public sealed class ProfilesEndpointDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        var group = app.MapGroup("/api/profiles").WithTags("Profiles");

        group.MapPost("register", handler: async ([FromServices] IMediator mediator, RegisterViewModel model, HttpContext context)
                => await mediator.Send(new RegisterAccount.Request(model), context.RequestAborted))
            .Produces(200)
            .WithOpenApi()
            .AllowAnonymous();
    }
}
