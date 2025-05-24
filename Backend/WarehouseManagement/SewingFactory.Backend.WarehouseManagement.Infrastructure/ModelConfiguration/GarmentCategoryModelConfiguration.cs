using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class GarmentCategoryModelConfiguration : IdentityModelConfigurationBase<GarmentCategory>
{
    protected override void AddBuilder(EntityTypeBuilder<GarmentCategory> builder) => builder.HasMany(x => x.Products)
        .WithOne(x => x.Category)
        .OnDelete(DeleteBehavior.Restrict);

    protected override string TableName() => "GarmentCategories";
}
