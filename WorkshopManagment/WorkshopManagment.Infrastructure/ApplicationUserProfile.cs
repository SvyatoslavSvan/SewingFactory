namespace SewingFactory.Backend.WorkshopManagement.Infrastructure;

/// <summary>
///     Represent person with login information (ApplicationUser)
/// </summary>
public class ApplicationUserProfile : Auditable
{
    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private ApplicationUserProfile()
    {
    }

    public ApplicationUserProfile(DateTime createdAt, string createdBy, ApplicationUser applicationUser, ICollection<MicroservicePermission>? permissions) : base(createdAt, createdBy)
    {
        ApplicationUser = applicationUser;
        Permissions = permissions;
    }

    /// <summary>
    ///     Account
    /// </summary>
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;

    /// <summary>
    ///     Microservice permission for policy-based authorization
    /// </summary>
    public ICollection<MicroservicePermission>? Permissions { get; set; }

    public Guid ApplicationUserId { get; set; }
}