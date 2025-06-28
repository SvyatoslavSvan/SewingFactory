using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Common.Domain.Base;
using System.Security.Claims;

namespace SewingFactory.Backend.IdentityServer.Infrastructure.DatabaseInitialization;

/// <summary>
///     Database Initializer
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    ///     Seeds one default users to database for demo purposes only
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static async void SeedUsers(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context!.Database.EnsureCreatedAsync();
        var pending = await context.Database.GetPendingMigrationsAsync();
        if (pending.Any())
        {
            await context!.Database.MigrateAsync();
        }

        if (context.Users.Any())
        {
            return;
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var users = new[]
        {
            new
            {
                User = new ApplicationUser
                {
                    UserName = "Designer",
                    Email = "SewingFactory@gmail.com",
                    NormalizedUserName = "DESIGNER",
                    NormalizedEmail = "SEWINGFACTORY@GMAIL.COM",
                    FirstName = "Clothing",
                    LastName = "Designer",
                    PhoneNumber = "+38000000000",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                Password = "Designer@123",
                Permissions = new[] { new AppPermission { PolicyName = AppData.DesignerAccess, Description = "Access policy for working with garment models and categories" } }
            },
            new
            {
                User = new ApplicationUser
                {
                    UserName = "Owner",
                    Email = "SewingFactory@gmail.com",
                    NormalizedUserName = "OWNER",
                    NormalizedEmail = "SEWINGFACTORY@GMAIL.COM",
                    FirstName = "Business",
                    LastName = "Owner",
                    PhoneNumber = "+38000000000",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                Password = "Owner@123",
                Permissions = new[]
                {
                    new AppPermission { PolicyName = AppData.DesignerAccess, Description = "Access policy for working with garment models and categories" },
                    new AppPermission { PolicyName = AppData.FinanceAccess, Description = "Access policy for working with finance" }
                }
            }
        };

        foreach (var entry in users)
        {
            if (await userManager.FindByNameAsync(entry.User.UserName) != null)
            {
                continue;
            }

            var createResult = await userManager.CreateAsync(entry.User, entry.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException($"Failed to create user {entry.User.UserName}: {errors}");
            }

            foreach (var perm in entry.Permissions)
            {
                var claim = new Claim(perm.PolicyName, "true");
                var claimResult = await userManager.AddClaimAsync(entry.User, claim);
                if (!claimResult.Succeeded)
                {
                    var err = string.Join("; ", claimResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException($"Failed to add claim {perm.PolicyName} to {entry.User.UserName}: {err}");
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
