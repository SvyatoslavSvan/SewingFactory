using Calabonga.AspNetCore.AppDefinitions;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.IdentityServer.Web.Definitions.Cors;

/// <summary>
///     Cors configurations
/// </summary>
public class CorsDefinition : AppDefinition
{
    /// <summary>
    ///     Configure services for current application
    /// </summary>
    /// <param name="builder"></param>
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var origins = builder.Configuration.GetSection("Cors").GetSection("Origins").Value?.Split(',');
        builder.Services.AddCors(setupAction: options =>
        {
            options.AddPolicy(AppData.PolicyCorsName, configurePolicy: policyBuilder =>
            {
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();

                if (origins is not { Length: > 0 })
                {
                    return;
                }

                if (origins.Contains("*"))
                {
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                    policyBuilder.SetIsOriginAllowed(isOriginAllowed: host => true);
                    policyBuilder.AllowCredentials();
                }
                else
                {
                    foreach (var origin in origins)
                    {
                        policyBuilder.WithOrigins(origin);
                    }
                }
            });
        });
    }
}
