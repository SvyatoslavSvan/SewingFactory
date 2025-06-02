using Microsoft.EntityFrameworkCore;
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
        builder.HasOne(navigationExpression: x => x.Department).WithMany(navigationExpression: x => x.Employees);
        builder.HasMany(navigationExpression: x => x.Documents).WithMany(navigationExpression: x => x.Employees);
        builder
            .Navigation(navigationExpression: e => e.Documents)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_documents");
    }
}