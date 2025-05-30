﻿#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SewingFactory.Backend.IdentityServer.Infrastructure.Base;

namespace SewingFactory.Backend.IdentityServer.Infrastructure
{
    /// <summary>
    /// Database context for current application
    /// </summary>
    public class ApplicationDbContext : DbContextBase
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }


        public DbSet<ApplicationUserProfile> Profiles { get; set; }

        public DbSet<AppPermission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseOpenIddict<Guid>();
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.ApplicationUserProfile)
                .WithOne(p => p.ApplicationUser)
                .HasForeignKey<ApplicationUser>(u => u.ApplicationUserProfileId);
            builder.Entity<ApplicationUserProfile>(b =>
            {
                b.HasKey(p => p.Id);                        
                b.Property(p => p.Id).ValueGeneratedOnAdd(); 
            });
            builder.Entity<AppPermission>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).ValueGeneratedOnAdd();
            });
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // It should be removed when using real Database (not in memory mode)
            optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            base.OnConfiguring(optionsBuilder);
        }
    }


    ///// <summary>
    ///// ATTENTION!
    ///// It should uncomment two line below when using real Database (not in memory mode). Don't forget update connection string.
    ///// </summary>
    //public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    //{
    //    public ApplicationDbContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    //        optionsBuilder.UseNpgsql("Server=<SQL>;Database=<DatabaseName>;User ID=<UserName>;Password=<Password>");
    //        return new ApplicationDbContext(optionsBuilder.Options);
    //    }
    //}

#nullable restore
}
