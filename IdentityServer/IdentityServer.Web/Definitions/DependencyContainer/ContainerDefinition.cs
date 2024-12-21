using Calabonga.AspNetCore.AppDefinitions;
using IdentityServer.Web.Application.Services;
using IdentityServer.Web.Definitions.Authorizations;

namespace IdentityServer.Web.Definitions.DependencyContainer;

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