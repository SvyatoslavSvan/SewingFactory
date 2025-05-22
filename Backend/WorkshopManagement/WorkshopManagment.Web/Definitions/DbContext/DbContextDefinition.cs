using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.DbContext;

/// <summary>
///     ASP.NET Core services registration and configurations
/// </summary>
public class DbContextDefinition : AppDefinition
{
    /// <summary>
    ///     Configure services for current microservice
    /// </summary>
    /// <param name="builder"></param>
    public override void ConfigureServices(WebApplicationBuilder builder)
        => builder.Services.AddDbContext<ApplicationDbContext>(optionsAction: config =>
        {
            config.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
        });
}