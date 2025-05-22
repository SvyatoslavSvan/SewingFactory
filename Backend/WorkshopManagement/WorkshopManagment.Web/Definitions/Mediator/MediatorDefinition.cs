using Calabonga.AspNetCore.AppDefinitions;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Web.Definitions.FluentValidating;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.Mediator;

/// <summary>
///     Register Mediator as MicroserviceDefinition
/// </summary>
public class MediatorDefinition : AppDefinition
{
    /// <summary>
    ///     Configure services for current microservice
    /// </summary>
    /// <param name="builder"></param>
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        builder.Services.AddMediatR(configuration: cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
    }
}