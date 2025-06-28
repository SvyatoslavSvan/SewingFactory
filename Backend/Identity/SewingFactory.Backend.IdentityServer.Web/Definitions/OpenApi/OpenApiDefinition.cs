using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Common.Domain.Base;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SewingFactory.Backend.IdentityServer.Web.Definitions.OpenApi;

/// <summary>
///     Swagger definition for application
/// </summary>
public sealed class OpenApiDefinition : AppDefinition
{
    public const string AppVersion = "9.0.6";

    private const string _openApiConfig = "/openapi/v1.json";

    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApiBehaviorOptions>(configureOptions: options => { options.SuppressModelStateInvalidFilter = true; });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi(configureOptions: options =>
        {
            options.AddDocumentTransformer<OAuth2SecuritySchemeTransformer>();
        });
    }

    public override void ConfigureApplication(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        app.MapOpenApi();

        app.UseSwaggerUI(setupAction: settings =>
        {
            settings.SwaggerEndpoint(_openApiConfig, $"{AppData.ServiceName} v.{AppVersion}");

            settings.DocumentTitle = $"{AppData.ServiceName}";
            settings.DefaultModelExpandDepth(0);
            settings.DefaultModelRendering(ModelRendering.Model);
            settings.DefaultModelsExpandDepth(0);
            settings.DocExpansion(DocExpansion.None);
            settings.OAuthScopeSeparator(" ");
            settings.OAuthClientId("client-id-code");
            settings.OAuthClientSecret("client-secret-code");
            settings.DisplayRequestDuration();
            settings.OAuthUsePkce();
            settings.OAuthAppName(AppData.ServiceName);
        });
    }
}
