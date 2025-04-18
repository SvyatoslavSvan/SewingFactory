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
        builder.Property(propertyExpression: x => x.Department).HasConversion<int>().IsRequired();
        builder.HasMany(navigationExpression: x => x.Tasks).WithOne(navigationExpression: x => x.Document).IsRequired();
        builder.Ignore(propertyExpression: x => x.Employees);
    }
}