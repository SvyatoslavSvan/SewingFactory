using Calabonga.AspNetCore.AppDefinitions;
using IdentityServer.Web.Application.Messaging.ProfileMessages.Queries;
using IdentityServer.Web.Application.Messaging.ProfileMessages.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Common.Domain.Base;

namespace IdentityServer.Web.Endpoints.ProfileEndpoints;

public sealed class ProfilesEndpointDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
        => app.MapProfilesEndpoints();
}

internal static class ProfilesEndpointDefinitionExtensions
{
    public static void MapProfilesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/profiles").WithTags("Profiles");

        group.MapGet("roles", handler: async ([FromServices] IMediator mediator, HttpContext context)
                => await mediator.Send(new GetProfile.Request(), context.RequestAborted))
            .RequireAuthorization(AppData.PolicyDefaultName)
            .RequireAuthorization(configurePolicy: x =>
            {
                x.RequireClaim("Profiles:Roles:Get");
            })
            .Produces(200)
            .ProducesProblem(401)
            .WithOpenApi();

        group.MapPost("register", handler: async ([FromServices] IMediator mediator, RegisterViewModel model, HttpContext context)
                => await mediator.Send(new RegisterAccount.Request(model), context.RequestAborted))
            .Produces(200)
            .WithOpenApi()
            .AllowAnonymous();
    }
}