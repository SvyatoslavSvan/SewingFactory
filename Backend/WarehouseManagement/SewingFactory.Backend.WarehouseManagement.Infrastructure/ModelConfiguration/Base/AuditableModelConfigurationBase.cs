using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

/// <summary>
///     Audit-able Model Configuration base
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AuditableModelConfigurationBase<T> : IEntityTypeConfiguration<T> where T : Auditable
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(TableName());
        builder.HasKey(keyExpression: x => x.Id);
        builder.Property(propertyExpression: x => x.Id).IsRequired();

        // audit
        builder.Property(propertyExpression: x => x.CreatedAt).IsRequired()
            .HasConversion(convertToProviderExpression: v => v, convertFromProviderExpression: v => DateTime.SpecifyKind(v, DateTimeKind.Utc)).IsRequired();

        builder.Property(propertyExpression: x => x.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(propertyExpression: x => x.UpdatedAt).HasConversion(convertToProviderExpression: v => v!.Value, convertFromProviderExpression: v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.Property(propertyExpression: x => x.UpdatedBy).HasMaxLength(256);

        AddBuilder(builder);
    }

    /// <summary>
    ///     Add custom properties for your entity
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void AddBuilder(EntityTypeBuilder<T> builder);

    /// <summary>
    ///     Table name
    /// </summary>
    /// <returns></returns>
    protected abstract string TableName();
}
