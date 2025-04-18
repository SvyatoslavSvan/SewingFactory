using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class EmployeeConfiguration : IdentityModelConfigurationBase<Employee>
{
    protected override string TableName() => "Employees";

    protected override void AddBuilder(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(propertyExpression: x => x.Name).IsRequired();
        builder.Property(propertyExpression: x => x.InternalId).IsRequired();
        builder.Property(propertyExpression: x => x.Department).HasConversion<int>().IsRequired();
    }
}