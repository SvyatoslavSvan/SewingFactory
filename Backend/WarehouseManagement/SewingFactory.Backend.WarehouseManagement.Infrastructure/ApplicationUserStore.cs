﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SewingFactory.Backend.WarehouseManagement.Infrastructure
{
    /// <summary>
    /// Application store for user
    /// </summary>
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>
    {
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null)
            : base(context, describer)
        {

        }

        /// <inheritdoc />
        public override Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
            => Users
                .Include(x => x.ApplicationUserProfile).ThenInclude(x => x!.Permissions)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId, cancellationToken: cancellationToken);
    }
}