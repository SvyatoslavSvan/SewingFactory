﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace SewingFactory.Backend.IdentityServer.Web.Definitions.OpenIddict;

/// <summary>
///     Policy provider
/// </summary>
public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;

    /// <inheritdoc />
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) => _options = options.Value;

    /// <inheritdoc />
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policyExists = await base.GetPolicyAsync(policyName);
        if (policyExists != null)
        {
            return policyExists;
        }

        policyExists = new AuthorizationPolicyBuilder().AddRequirements(new PermissionRequirement(policyName)).Build();
        _options.AddPolicy(policyName, policyExists);

        return policyExists;
    }
}
