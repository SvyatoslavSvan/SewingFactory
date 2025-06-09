using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class PointOfSaleModelConfiguration : IdentityModelConfigurationBase<PointOfSale>
{
    protected override void AddBuilder(EntityTypeBuilder<PointOfSale> builder) => builder.HasMany(navigationExpression: x => x.StockItems)
        .WithOne(navigationExpression: x => x.PointOfSale)
        .OnDelete(DeleteBehavior.Cascade);

    protected override string TableName() => "PointsOfSale";
}
