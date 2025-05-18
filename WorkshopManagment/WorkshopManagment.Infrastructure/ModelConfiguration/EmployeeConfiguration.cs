using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class EmployeeConfiguration : IdentityModelConfigurationBase<Employee>
{
    protected override string TableName() => "Employees";

    protected override void AddBuilder(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(propertyExpression: x => x.Name).IsRequired();
        builder.Property(propertyExpression: x => x.InternalId).IsRequired();
        builder.HasOne(x => x.Department).WithMany(x => x.Employees);
        builder.HasMany(x => x.Documents).WithMany(x => x.Employees);
    }
}