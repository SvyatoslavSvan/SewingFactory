#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure;

/// <summary>
///     Database context for current application
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContextBase(options)
{
    public DbSet<ApplicationUserProfile> Profiles { get; set; }

    public DbSet<MicroservicePermission> Permissions { get; set; }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<ProcessBasedEmployee> ProcessBasedEmployees { get; set; }
    public DbSet<RateBasedEmployee> RateBasedEmployees { get; set; }
    public DbSet<Technologist> Technologists { get; set; }
    public DbSet<EmployeeTaskRepeat> EmployeeTaskRepeats { get; set; }
    public DbSet<GarmentCategory> GarmentCategories { get; set; }
    public DbSet<GarmentModel> GarmentModels { get; set; }
    public DbSet<Process> Processes { get; set; }
    public DbSet<Timesheet> Timesheets { get; set; }
    public DbSet<WorkDay> WorkDays { get; set; }
    public DbSet<WorkshopDocument> WorkshopDocuments { get; set; }
    public DbSet<WorkshopTask> WorkshopTasks { get; set; }
    public DbSet<Department> Departments { get; set; }
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
        optionsBuilder.ConfigureWarnings(warningsConfigurationBuilderAction: x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Employee>().ToTable("Employees");
        builder.Entity<ProcessBasedEmployee>().ToTable("ProcessBasedEmployees");
        builder.Entity<RateBasedEmployee>().ToTable("RateBasedEmployees");
        builder.Entity<Technologist>().ToTable("Technologists");

        builder.Entity<ProcessBasedEmployee>()
            .HasMany(navigationExpression: e => e.Documents)
            .WithMany(navigationExpression: d => d.Employees);
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