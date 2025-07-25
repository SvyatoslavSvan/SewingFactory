﻿namespace SewingFactory.Backend.WarehouseManagement.Infrastructure;

/// <summary>
///     Default user for application.
///     Add profile data for application users by adding properties to the ApplicationUser class
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    ///     FirstName
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    ///     LastName
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    ///     Profile identity
    /// </summary>
    public Guid? ApplicationUserProfileId { get; set; }

    /// <summary>
    ///     User Profile
    /// </summary>
    public virtual ApplicationUserProfile? ApplicationUserProfile { get; set; }
}
