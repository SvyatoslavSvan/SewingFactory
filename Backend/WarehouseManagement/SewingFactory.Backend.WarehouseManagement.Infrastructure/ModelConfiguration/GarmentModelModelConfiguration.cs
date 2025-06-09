using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class GarmentModelModelConfiguration : IdentityModelConfigurationBase<GarmentModel>
{
    protected override void AddBuilder(EntityTypeBuilder<GarmentModel> builder)
    {
        builder.OwnsOne(navigationExpression: x => x.Price);

        builder.Property<Guid>("CategoryId").IsRequired();

        builder.HasOne(navigationExpression: x => x.Category)
            .WithMany(navigationExpression: x => x.Products)
            .HasForeignKey("CategoryId")
            .OnDelete(DeleteBehavior.Restrict);
    }

    protected override string TableName() => "GarmentModels";
}
