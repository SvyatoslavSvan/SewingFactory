using IdentityServer.Infrastructure.ModelConfiguration.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Infrastructure.ModelConfiguration;

public class ApplicationUserProfileConfiguration : AuditableModelConfigurationBase<ApplicationUserProfile>
{
    protected override void AddBuilder(EntityTypeBuilder<ApplicationUserProfile> builder) => builder.HasOne(x => x.ApplicationUser);

    protected override string TableName() => nameof(ApplicationUserProfile);
}


