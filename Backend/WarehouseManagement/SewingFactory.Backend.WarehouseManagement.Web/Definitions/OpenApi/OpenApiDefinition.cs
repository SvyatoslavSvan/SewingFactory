﻿using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Common.Domain.Base;
using Swashbuckle.AspNetCore.SwaggerUI;

#pragma warning disable CS0618

namespace SewingFactory.Backend.WarehouseManagement.Web.Definitions.OpenApi;

/// <summary>
///     Swagger definition for application
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class OpenApiDefinition : AppDefinition
{
    // -------------------------------------------------------
    // ATTENTION!
    // -------------------------------------------------------
    // If you use are git repository then you can uncomment line with "ThisAssembly" below for versioning by GIT possibilities.
    // Otherwise, you can change versions of your API by manually.
    // If you are not going to use git-versioning, do not forget install package "GitInfo" 
    public const string AppVersion = $"{ThisAssembly.Git.SemVer.Major}.{ThisAssembly.Git.SemVer.Minor}.{ThisAssembly.Git.SemVer.Patch}";
    // -------------------------------------------------------


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

        var url = app.Services.GetRequiredService<IConfiguration>().GetValue<string>("AuthServer:Url");

        app.MapOpenApi().AllowAnonymous();

        app.UseSwaggerUI(setupAction: settings =>
        {
            settings.SwaggerEndpoint(_openApiConfig, $"{AppData.ServiceName} v.{AppVersion}");

            // ATTENTION!
            // If you use are git repository then you can uncomment line with "ThisAssembly" below for versioning by GIT possibilities.
            settings.HeadContent = $"{ThisAssembly.Git.Branch.ToUpper()} {ThisAssembly.Git.Commit.ToUpper()}";

            settings.DocumentTitle = $"{AppData.ServiceName}";
            settings.DefaultModelExpandDepth(0);
            settings.DefaultModelRendering(ModelRendering.Model);
            settings.DefaultModelsExpandDepth(0);
            settings.DocExpansion(DocExpansion.None);
            settings.OAuthScopeSeparator(" ");
            settings.OAuthClientId("client-id-code");
            settings.OAuthClientSecret("client-secret-code");
            settings.DisplayRequestDuration();
            settings.OAuthAppName(AppData.ServiceName);
            settings.OAuthUsePkce();
        });
    }
}
