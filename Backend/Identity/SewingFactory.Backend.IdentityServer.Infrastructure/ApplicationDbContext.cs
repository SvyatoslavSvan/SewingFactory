#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SewingFactory.Backend.IdentityServer.Infrastructure.Base;

namespace SewingFactory.Backend.IdentityServer.Infrastructure;

/// <summary>
///     Database context for current application
/// </summary>
public class ApplicationDbContext : DbContextBase
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public DbSet<ApplicationUserProfile> Profiles { get; set; }

    public DbSet<AppPermission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.UseOpenIddict<Guid>();
        builder.Entity<ApplicationUser>()
            .HasOne(navigationExpression: u => u.ApplicationUserProfile)
            .WithOne(navigationExpression: p => p.ApplicationUser)
            .HasForeignKey<ApplicationUser>(foreignKeyExpression: u => u.ApplicationUserProfileId);

        builder.Entity<ApplicationUserProfile>(buildAction: b =>
        {
            b.HasKey(keyExpression: p => p.Id);
            b.Property(propertyExpression: p => p.Id).ValueGeneratedOnAdd();
        });

        builder.Entity<AppPermission>(buildAction: b =>
        {
            b.HasKey(keyExpression: p => p.Id);
            b.Property(propertyExpression: p => p.Id).ValueGeneratedOnAdd();
        });

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // It should be removed when using real Database (not in memory mode)
        optionsBuilder.ConfigureWarnings(warningsConfigurationBuilderAction: x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        base.OnConfiguring(optionsBuilder);
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Auth;Username=postgres;Password=1234;Include Error Detail=true");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
