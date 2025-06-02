using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class ProcessConfiguration : IdentityModelConfigurationBase<Process>
{
    protected override string TableName() => "Processes";

    protected override void AddBuilder(EntityTypeBuilder<Process> builder)
    {
        builder.Property(propertyExpression: x => x.Name).IsRequired();
        builder.OwnsOne(navigationExpression: x => x.Price, buildAction: b =>
        {
            b.Property(propertyExpression: p => p.Amount).HasColumnName("PriceAmount");
        });

        builder.HasOne(navigationExpression: x => x.Department).WithMany(navigationExpression: x => x.Processes);
    }
}