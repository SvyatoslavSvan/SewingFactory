using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SewingFactory.Backend.WorkshopManagement.Infrastructure;

/// <summary>
///     Application store for user
/// </summary>
public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>
{
    public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {
    }

    /// <inheritdoc />
    public override Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = new())
        => Users
            .Include(navigationPropertyPath: x => x.ApplicationUserProfile).ThenInclude(navigationPropertyPath: x => x!.Permissions)
            .FirstOrDefaultAsync(predicate: u => u.Id.ToString() == userId, cancellationToken);
}