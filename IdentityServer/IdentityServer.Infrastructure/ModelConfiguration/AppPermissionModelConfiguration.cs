using IdentityServer.Infrastructure.ModelConfiguration.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Infrastructure.ModelConfiguration
{
    public class AppPermissionModelConfiguration : AuditableModelConfigurationBase<AppPermission>
    {
        protected override void AddBuilder(EntityTypeBuilder<AppPermission> builder)
        {
            builder.Property(x => x.PolicyName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);
        }

        protected override string TableName() => nameof(AppPermission);
    }
}
