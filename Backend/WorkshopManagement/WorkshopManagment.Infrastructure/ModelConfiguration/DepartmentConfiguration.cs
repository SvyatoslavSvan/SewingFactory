using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

internal class DepartmentConfiguration : IdentityModelConfigurationBase<Department>
{
    protected override void AddBuilder(EntityTypeBuilder<Department> builder)
    {
        builder.Property(propertyExpression: x => x.Name).IsRequired();
        builder.HasMany(navigationExpression: d => d.Employees).WithOne(navigationExpression: e => e.Department);
        builder.HasMany(navigationExpression: d => d.Documents).WithOne(navigationExpression: w => w.Department);
        builder.HasMany(navigationExpression: d => d.Processes).WithOne(navigationExpression: p => p.Department);
    }

    protected override string TableName() => "Departments";
}