using SewingFactory.Common.Domain.Base;

namespace IdentityServer.Infrastructure;

/// <summary>
///     Represent person with login information (ApplicationUser)
/// </summary>
public sealed class ApplicationUserProfile : Auditable
{
    /// <summary>
    /// Empty ctor for ORM
    /// </summary>
    public ApplicationUserProfile() { }
    /// <summary>
    ///     Represent person with login information (ApplicationUser)
    /// </summary>
    public ApplicationUserProfile(DateTime createdAt, string createdBy, ApplicationUser? applicationUser, List<AppPermission>? permissions) : base(createdAt, createdBy)
    {
        ApplicationUser = applicationUser;
        Permissions = permissions;
    }

    /// <summary>
    ///     Account
    /// </summary>
    public ApplicationUser? ApplicationUser { get; set; }

    /// <summary>
    ///     Application permission for policy-based authorization
    /// </summary>
    public List<AppPermission>? Permissions { get; set; }
}