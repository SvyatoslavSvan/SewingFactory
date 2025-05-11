using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class TechnologistConfiguration : IEntityTypeConfiguration<Technologist>
{
    public void Configure(EntityTypeBuilder<Technologist> builder)
    {
        builder.Property(propertyExpression: x => x.Name).IsRequired();
        builder.Property(propertyExpression: x => x.InternalId).IsRequired();
        builder.Property(propertyExpression: x => x.SalaryPercentage)
            .HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new Percent(v))
            .IsRequired();
    }
}