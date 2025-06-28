using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using SewingFactory.Backend.IdentityServer.Infrastructure;
using SewingFactory.Backend.IdentityServer.Web.Definitions.Authorizations;

namespace SewingFactory.Backend.IdentityServer.Web.Definitions.DbContext;

/// <summary>
///     ASP.NET Core services registration and configurations
/// </summary>
public class DbContextDefinition : AppDefinition
{
    /// <summary>
    ///     Configure services for current application
    /// </summary>
    /// <param name="builder"></param>
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(optionsAction: config =>
        {
            config.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            config.UseOpenIddict<Guid>();
        });

        builder.Services.Configure<IdentityOptions>(configureOptions: options =>
        {
            options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email;
            // configure more options if you need
        });

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(setupAction: options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
            })
            .AddSignInManager()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<ApplicationUserStore>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        builder.Services.AddTransient<ApplicationUserStore>();
    }
}
