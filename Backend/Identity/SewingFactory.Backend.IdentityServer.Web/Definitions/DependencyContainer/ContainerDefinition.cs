using Calabonga.AspNetCore.AppDefinitions;
using SewingFactory.Backend.IdentityServer.Web.Application.Services;
using SewingFactory.Backend.IdentityServer.Web.Definitions.Authorizations;

namespace SewingFactory.Backend.IdentityServer.Web.Definitions.DependencyContainer;

/// <summary>
///     Dependency container definition
/// </summary>
public class ContainerDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IAccountService, AccountService>();
        builder.Services.AddTransient<ApplicationUserClaimsPrincipalFactory>();
    }
}
