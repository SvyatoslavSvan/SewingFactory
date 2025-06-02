using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class TimesheetConfiguration : IdentityModelConfigurationBase<Timesheet>
{
    protected override string TableName() => "Timesheets";

    protected override void AddBuilder(EntityTypeBuilder<Timesheet> builder)
    {
        builder.Property(propertyExpression: x => x.Hours).IsRequired();
        builder.Property(propertyExpression: x => x.DaysCount).IsRequired();
        builder.Property(propertyExpression: x => x.Date).IsRequired();
        builder.HasMany(navigationExpression: x => x.Employees).WithMany(navigationExpression: x => x.Timesheets);
    }
}