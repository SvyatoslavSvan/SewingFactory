using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class StockItemModelConfiguration : IdentityModelConfigurationBase<StockItem>
{
    protected override void AddBuilder(
        EntityTypeBuilder<StockItem> builder)
    {
        builder.Property(propertyExpression: x => x.Quantity)
            .IsRequired();

        builder.Property<Guid>("PointOfSaleId")
            .IsRequired();

        builder.Property<Guid>("GarmentModelId")
            .IsRequired();

        builder.Ignore(x => x.SumOfStock);
        builder.HasOne(navigationExpression: x => x.PointOfSale)
            .WithMany(navigationExpression: x => x.StockItems)
            .HasForeignKey("PointOfSaleId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Operations)
            .WithOne(x => x.StockItem)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(navigationExpression: x => x.GarmentModel)
            .WithMany()
            .HasForeignKey("GarmentModelId")
            .OnDelete(DeleteBehavior.Restrict);
    }

    protected override string TableName()
        => "StockItems";
}
