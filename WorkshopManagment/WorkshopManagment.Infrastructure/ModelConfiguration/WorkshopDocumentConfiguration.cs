using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class WorkshopDocumentConfiguration : IdentityModelConfigurationBase<WorkshopDocument>
{
    protected override string TableName() => "WorkshopDocuments";

    protected override void AddBuilder(EntityTypeBuilder<WorkshopDocument> builder)
    {
        builder.Property(propertyExpression: x => x.CountOfModelsInvolved).IsRequired();
        builder.Property(propertyExpression: x => x.Date).IsRequired();
        builder.HasMany(navigationExpression: x => x.Tasks).WithOne(navigationExpression: x => x.Document).IsRequired();
        builder.HasMany(x => x.Employees).WithMany(x => x.Documents);
        builder.HasOne(w => w.Department).WithMany(d => d.Documents);
        builder.Navigation(d => d.Tasks)
            .UsePropertyAccessMode(PropertyAccessMode.Field).HasField("_tasks");
        builder.Navigation(d => d.Employees)
            .UsePropertyAccessMode(PropertyAccessMode.Field).HasField("_employees");
    }
}