using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class RateBasedEmployeeConfiguration : IEntityTypeConfiguration<RateBasedEmployee>
{
    public void Configure(EntityTypeBuilder<RateBasedEmployee> builder)
    {
        builder.Property(propertyExpression: x => x.Name).IsRequired();
        builder.Property(propertyExpression: x => x.InternalId).IsRequired();
        builder.Property(propertyExpression: x => x.Department).HasConversion<int>().IsRequired();
        builder.Property(propertyExpression: x => x.Premium)
            .HasConversion(convertToProviderExpression: v => v.Value, convertFromProviderExpression: v => new Percent(v))
            .IsRequired();

        builder.Property(propertyExpression: x => x.Rate)
            .HasConversion(convertToProviderExpression: v => v.Amount, convertFromProviderExpression: v => new Money(v))
            .IsRequired();
    }
}