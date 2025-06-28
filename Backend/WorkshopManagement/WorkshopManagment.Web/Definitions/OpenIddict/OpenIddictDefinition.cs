using Calabonga.AspNetCore.AppDefinitions;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.OpenIddict;

public sealed class OpenIddictDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder) =>
        builder.Services
            .AddOpenIddict()
            .AddValidation(configuration: options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });
}