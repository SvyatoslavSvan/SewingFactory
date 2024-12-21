using Microsoft.AspNetCore.Authentication.Cookies;
using OpenIddict.Validation.AspNetCore;

namespace IdentityServer.Web.Definitions.OpenIddict;

public static class AuthData
{
    public const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
}