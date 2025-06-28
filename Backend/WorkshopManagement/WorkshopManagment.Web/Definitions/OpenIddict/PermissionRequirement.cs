using Microsoft.AspNetCore.Authorization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.OpenIddict;

/// <summary>
///     Permission requirement for user or service authorization
/// </summary>
public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permissionName) => PermissionName = permissionName;

    /// <summary>
    ///     Permission name
    /// </summary>
    public string PermissionName { get; }
}