namespace SewingFactory.Backend.WarehouseManagment.Infrastructure
{
    /// <summary>
    /// User permission for microservice
    /// </summary>
    public class MicroservicePermission : Auditable
    {
        public MicroservicePermission(DateTime createdAt, string createdBy, Guid applicationUserProfileId, ApplicationUserProfile? applicationUserProfile, string policyName, string? description) : base(createdAt, createdBy)
        {
            ApplicationUserProfileId = applicationUserProfileId;
            ApplicationUserProfile = applicationUserProfile;
            PolicyName = policyName;
            Description = description;
        }

        /// <summary>
        /// Application User profile identifier
        /// </summary>
        public Guid ApplicationUserProfileId { get; set; }

        /// <summary>
        /// Application User Profile
        /// </summary>
        public virtual ApplicationUserProfile? ApplicationUserProfile { get; set; }

        /// <summary>
        /// Authorize attribute policy name
        /// </summary>
        public string PolicyName { get; set; } = null!;

        /// <summary>
        /// Description for current permission
        /// </summary>
        public string? Description { get; set; }
    }
}