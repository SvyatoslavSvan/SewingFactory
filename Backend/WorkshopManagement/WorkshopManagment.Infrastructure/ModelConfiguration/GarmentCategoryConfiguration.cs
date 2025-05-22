using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure.ModelConfiguration;

public class GarmentCategoryConfiguration : IdentityModelConfigurationBase<GarmentCategory>
{
    protected override string TableName() => "GarmentCategories";

    protected override void AddBuilder(EntityTypeBuilder<GarmentCategory> builder)
    {
        builder.Property(propertyExpression: x => x.Name).IsRequired();
        builder.HasMany(navigationExpression: x => x.GarmentModels).WithOne(navigationExpression: x => x.Category).IsRequired();
    }
}