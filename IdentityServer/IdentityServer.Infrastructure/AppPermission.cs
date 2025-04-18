using SewingFactory.Common.Domain.Base;

namespace IdentityServer.Infrastructure;

/// <summary>
///     User permission for application
/// </summary>
public sealed class AppPermission : Auditable
{
    /// <summary>
    /// Empty ctor for ORM
    /// </summary>
    public AppPermission() { }

    public AppPermission(DateTime createdAt, string createdBy, Guid applicationUserProfileId, ApplicationUserProfile? applicationUserProfile, string policyName, string description) : base(createdAt,
        createdBy)
    {
        ApplicationUserProfileId = applicationUserProfileId;
        ApplicationUserProfile = applicationUserProfile;
        PolicyName = policyName;
        Description = description;
    }

    /// <summary>
    ///     Application User profile identifier
    /// </summary>
    public Guid ApplicationUserProfileId { get; set; }

    /// <summary>
    ///     Application User Profile
    /// </summary>
    public ApplicationUserProfile? ApplicationUserProfile { get; set; }

    /// <summary>
    ///     Authorize attribute policy name
    /// </summary>
    public string PolicyName { get; set; } = null!;

    /// <summary>
    ///     Description for current permission
    /// </summary>
    public string Description { get; set; } = null!;
}