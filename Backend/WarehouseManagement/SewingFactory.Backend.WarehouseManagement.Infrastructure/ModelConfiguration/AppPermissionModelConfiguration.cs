using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class AppPermissionModelConfiguration : AuditableModelConfigurationBase<MicroservicePermission>
{
    protected override void AddBuilder(EntityTypeBuilder<MicroservicePermission> builder)
    {
        builder.Property(propertyExpression: x => x.PolicyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(propertyExpression: x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);
    }

    protected override string TableName() => nameof(MicroservicePermission);
}
