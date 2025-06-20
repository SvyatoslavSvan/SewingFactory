﻿using Calabonga.AspNetCore.AppDefinitions;
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
            // UseInMemoryDatabase - This for demo purposes only!
            // Should uninstall package "Microsoft.EntityFrameworkCore.InMemory" and install what you need.
            // For example: "Microsoft.EntityFrameworkCore.SqlServer"
            config.UseInMemoryDatabase("DEMO-PURPOSES-ONLY");

            // uncomment line below to use UseNpgsql() or UseSqlServer(). Don't forget setup connection string in appSettings.json
            //config.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)));

            // Register the entity sets needed by OpenIddict.
            // Note: use the generic overload if you need to replace the default OpenIddict entities.
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
