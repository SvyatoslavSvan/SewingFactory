﻿#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagment.Infrastructure.Base;

namespace SewingFactory.Backend.WarehouseManagment.Infrastructure
{
    /// <summary>
    /// Database context for current application
    /// </summary>
    public class ApplicationDbContext : DbContextBase
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<ApplicationUserProfile> Profiles { get; set; }

        public DbSet<MicroservicePermission> Permissions { get; set; }

        public DbSet<GarmentCategory> GarmentCategories { get; set; }

        public DbSet<GarmentModel> GarmentModels { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<PointOfSale> PointOfSales { get; set; }

        public DbSet<StockItem> StockItems { get; set; }

        /// <summary>
        ///     <para>
        ///         Override this method to configure the database (and other options) to be used for this context.
        ///         This method is called for each instance of the context that is created.
        ///         The base implementation does nothing.
        ///     </para>
        ///     <para>
        ///         In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        ///         to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        ///         the options have already been set, and skip some or all of the logic in
        ///         <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
        ///     for more information.
        /// </remarks>
        /// <param name="optionsBuilder">
        ///     A builder used to create or modify options for this context. Databases (and other extensions)
        ///     typically define extension methods on this object that allow you to configure the context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder) => builder.Entity<Operation>()
            .ToTable("Operations")
            .HasDiscriminator<string>("OperationType")
            .HasValue<Operation>("Operation")
            .HasValue<InternalTransferOperation>("InternalTransferOperation");
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
//        optionsBuilder.UseSqlServer("Server=<SQL>;Database=<DatabaseName>;User ID=<UserName>;Password=<Password>");
//        return new ApplicationDbContext(optionsBuilder.Options);
//    }
//}

#nullable restore