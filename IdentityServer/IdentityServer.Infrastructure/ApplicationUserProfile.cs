using SewingFactory.Common.Domain.Base;

namespace IdentityServer.Infrastructure;

/// <summary>
///     Represent person with login information (ApplicationUser)
/// </summary>
public sealed class ApplicationUserProfile(DateTime createdAt, string createdBy, ApplicationUser? applicationUser, List<AppPermission>? permissions)
    : Auditable(createdAt, createdBy)
{
    /// <summary>
    ///     Account
    /// </summary>
    public ApplicationUser? ApplicationUser { get; set; } = applicationUser;

    /// <summary>
    ///     Application permission for policy-based authorization
    /// </summary>
    public List<AppPermission>? Permissions { get; set; } = permissions;
}