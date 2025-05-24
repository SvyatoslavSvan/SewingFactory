using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class PointOfSaleModelConfiguration : IdentityModelConfigurationBase<PointOfSale>
{
    protected override void AddBuilder(EntityTypeBuilder<PointOfSale> builder) => builder.HasMany(x => x.StockItems)
        .WithOne(x => x.PointOfSale)
        .OnDelete(DeleteBehavior.Cascade);

    protected override string TableName() => "PointsOfSale";
}
