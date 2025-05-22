using Calabonga.AspNetCore.AppDefinitions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.IdentityServer.Domain.Base;
using SewingFactory.Backend.IdentityServer.Web.Application.Messaging.ProfileMessages.Queries;
using SewingFactory.Backend.IdentityServer.Web.Application.Messaging.ProfileMessages.ViewModels;

namespace SewingFactory.Backend.IdentityServer.Web.Endpoints
{
    public sealed class ProfilesEndpointDefinition : AppDefinition
    {
        public override void ConfigureApplication(WebApplication app)
        {
            var group = app.MapGroup("/api/profiles").WithTags("Profiles");

            group.MapGet("roles", async ([FromServices] IMediator mediator, HttpContext context)
                    => await mediator.Send(new GetProfile.Request(), context.RequestAborted))
                .RequireAuthorization(AppData.PolicyDefaultName)
                .RequireAuthorization(x =>
                {
                    x.RequireClaim("Profiles:Roles:Get");
                })
                .Produces(200)
                .ProducesProblem(401)
                .WithOpenApi();

            group.MapPost("register", async ([FromServices] IMediator mediator, RegisterViewModel model, HttpContext context)
                    => await mediator.Send(new RegisterAccount.Request(model), context.RequestAborted))
                .Produces(200)
                .WithOpenApi()
                .AllowAnonymous();
        }
    }
}