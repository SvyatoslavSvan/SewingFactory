using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure.ModelConfiguration;

public class ApplicationUserProfileConfiguration : AuditableModelConfigurationBase<ApplicationUserProfile>
{
    protected override void AddBuilder(EntityTypeBuilder<ApplicationUserProfile> builder) => builder.HasOne(navigationExpression: x => x.ApplicationUser)
        .WithOne(navigationExpression: u => u.ApplicationUserProfile)
        .HasForeignKey<ApplicationUserProfile>(foreignKeyExpression: x => x.ApplicationUserId);

    protected override string TableName() => nameof(ApplicationUserProfile);
}
