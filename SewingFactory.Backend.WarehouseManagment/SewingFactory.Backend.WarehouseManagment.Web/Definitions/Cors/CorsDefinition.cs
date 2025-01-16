using Calabonga.AspNetCore.AppDefinitions;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WarehouseManagment.Web.Definitions.Cors
{
    /// <summary>
    /// Cors configurations
    /// </summary>
    public class CorsDefinition : AppDefinition
    {
        /// <summary>
        /// Configure services for current microservice
        /// </summary>
        /// <param name="builder"></param>
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            var origins = builder.Configuration.GetSection("Cors")?.GetSection("Origins")?.Value?.Split(',');
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AppData.PolicyName, builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    if (origins is not { Length: > 0 })
                    {
                        return;
                    }

                    if (origins.Contains("*"))
                    {
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.SetIsOriginAllowed(host => true);
                        builder.AllowCredentials();
                    }
                    else
                    {
                        foreach (var origin in origins)
                        {
                            builder.WithOrigins(origin);
                        }
                    }
                });
            });
        }
    }
}