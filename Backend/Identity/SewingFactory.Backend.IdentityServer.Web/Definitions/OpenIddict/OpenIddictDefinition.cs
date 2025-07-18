﻿using Calabonga.AspNetCore.AppDefinitions;
using OpenIddict.Abstractions;
using SewingFactory.Backend.IdentityServer.Infrastructure;
using SewingFactory.Backend.IdentityServer.Web.HostedServices;

namespace SewingFactory.Backend.IdentityServer.Web.Definitions.OpenIddict;

public class OpenIddictDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddOpenIddict()
            // Register the OpenIddict core components.
            .AddCore(configuration: options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default entities.
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>()
                    .ReplaceDefaultEntities<Guid>();
            })

            // Register the OpenIddict server components.
            .AddServer(configuration: options =>
            {
                // Note: the sample uses the code and refresh token flows but you can enable
                // the other flows if you need to support implicit, password or client credentials.
                // Supported flows are:
                //  => Authorization code flow
                //  => Client credentials flow
                //  => Device code flow
                //  => Implicit flow
                //  => Password flow
                //  => Refresh token flow
                options
                    .AllowAuthorizationCodeFlow().RequireProofKeyForCodeExchange()
                    .AllowClientCredentialsFlow()
                    .AllowRefreshTokenFlow();

                // Using reference tokens means the actual access and refresh tokens
                // are stored in the database and different tokens, referencing the actual
                // tokens (in the db), are used in request headers. The actual tokens are not
                // made public.
                // => options.UseReferenceAccessTokens();
                // => options.UseReferenceRefreshTokens();

                // Set the lifetime of your tokens
                // => options.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
                // => options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

                // Enable the token endpoint.
                options.SetAuthorizationEndpointUris("connect/authorize").RequireProofKeyForCodeExchange() // enable PKCE
                    //.SetDeviceEndpointUris("connect/device")
                    .SetIntrospectionEndpointUris("connect/introspect")
                    .SetEndSessionEndpointUris("connect/logout")
                    .SetTokenEndpointUris("connect/token")
                    //.SetVerificationEndpointUris("connect/verify"),
                    .SetUserInfoEndpointUris("connect/userinfo");

                // Encryption and signing of tokens
                options
                    .AddEphemeralEncryptionKey() // only for Developing mode
                    .AddEphemeralSigningKey() // only for Developing mode
                    .DisableAccessTokenEncryption(); // only for Developing mode

                // Mark the "email", "profile" and "roles" scopes as supported scopes.
                options.RegisterScopes(
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Roles,
                    "api",
                    "custom",
                    OpenIddictConstants.Scopes.OfflineAccess);

                // Register the signing and encryption credentials.
                options
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options
                    .UseAspNetCore()
                    .EnableEndSessionEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .DisableTransportSecurityRequirement();

                // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                // JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

                //options.AddEventHandler<OpenIddictServerEvents.ProcessSignInContext>(builder =>
                //{
                //    builder.SetOrder(OpenIddictServerHandlers.GenerateIdentityModelRefreshToken.Descriptor.Order - 1)
                //        .AddFilter<OpenIddictServerHandlerFilters.RequireRefreshTokenGenerated>()
                //        .SetType(OpenIddictServerHandlerType.Custom)
                //        .UseInlineHandler(context =>
                //        {
                //            context.RefreshTokenPrincipal = context.RefreshTokenPrincipal.Clone(
                //                claim => claim.Type is (
                //                    OpenIddictConstants.Claims.Private.AuthorizationId or
                //                    OpenIddictConstants.Claims.Private.Presenter or
                //                    OpenIddictConstants.Claims.Private.TokenId or
                //                    OpenIddictConstants.Claims.Private.Scope or
                //                    OpenIddictConstants.Claims.Subject or
                //                    OpenIddictConstants.Claims.ExpiresAt
                //                    )
                //            );
                //            return default;
                //        });
                //});
            })

            // Register the OpenIddict validation components.
            .AddValidation(configuration: options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

        // Register the worker responsible for seeding the database.
        // Note: in a real world application, this step should be part of a setup script.
        builder.Services.AddHostedService<OpenIddictWorker>();
    }
}
