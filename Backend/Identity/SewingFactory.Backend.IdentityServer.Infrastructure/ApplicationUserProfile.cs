using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.IdentityServer.Infrastructure;

/// <summary>
///     Represent person with login information (ApplicationUser)
/// </summary>
public sealed class ApplicationUserProfile : Auditable
{
    /// <summary>
    ///     Account
    /// </summary>
    public ApplicationUser? ApplicationUser { get; set; }

    /// <summary>
    ///     Application permission for policy-based authorization
    /// </summary>
    public List<AppPermission>? Permissions { get; set; }
}
