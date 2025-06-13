using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class GarmentModelConfiguration : IdentityModelConfigurationBase<GarmentModel>
{
    protected override string TableName() => "GarmentModels";

    protected override void AddBuilder(EntityTypeBuilder<GarmentModel> builder)
    {
        builder.OwnsOne(navigationExpression: x => x.Price);
        builder.Property(propertyExpression: x => x.Name).IsRequired();
        builder.Property(propertyExpression: x => x.Description).IsRequired();
        builder.HasOne(navigationExpression: x => x.Category).WithMany(navigationExpression: x => x.GarmentModels).IsRequired();
    }
}