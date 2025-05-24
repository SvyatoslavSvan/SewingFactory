using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure
{
    /// <summary>
    /// Represent person with login information (ApplicationUser)
    /// </summary>
    public class ApplicationUserProfile : Auditable
    {
        /// <summary>
        /// Account
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        /// <summary>
        /// Microservice permission for policy-based authorization
        /// </summary>
        public ICollection<MicroservicePermission>? Permissions { get; set; }

        public Guid ApplicationUserId { get; set; }
    }
}
