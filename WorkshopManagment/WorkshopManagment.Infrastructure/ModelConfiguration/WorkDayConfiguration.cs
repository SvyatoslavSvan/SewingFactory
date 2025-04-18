using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class WorkDayConfiguration : IdentityModelConfigurationBase<WorkDay>
{
    protected override string TableName() => "WorkDays";

    protected override void AddBuilder(EntityTypeBuilder<WorkDay> builder)
    {
        builder.Property(propertyExpression: x => x.Hours).IsRequired();
        builder.Property(propertyExpression: x => x.Date).IsRequired();
        builder.HasOne(navigationExpression: x => x.Employee).WithMany().IsRequired();
    }
}