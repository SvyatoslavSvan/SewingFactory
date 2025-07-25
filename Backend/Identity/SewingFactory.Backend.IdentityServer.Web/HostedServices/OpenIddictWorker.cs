﻿using OpenIddict.Abstractions;
using SewingFactory.Backend.IdentityServer.Infrastructure;

namespace SewingFactory.Backend.IdentityServer.Web.HostedServices;

public sealed class OpenIddictWorker(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        // credentials password
        const string client_id1 = "client-id-sts";
        if (await manager.FindByClientIdAsync(client_id1, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = client_id1,
                ClientSecret = "client-secret-sts",
                DisplayName = "Service-To-Service demonstration",
                Permissions =
                {
                    // Endpoint permissions
                    OpenIddictConstants.Permissions.Endpoints.Token,

                    // Grant type permissions
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.GrantTypes.Password,

                    // Scope permissions
                    OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                }
            }, cancellationToken);
        }

        const string client_id2 = "client-id-code";
        if (await manager.FindByClientIdAsync(client_id2, cancellationToken) is null)
        {
            var url = serviceProvider.GetRequiredService<IConfiguration>().GetValue<string>("AuthServer:Url");

            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = client_id2,
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                ClientSecret = "client-secret-code",
                DisplayName = "API testing clients with Authorization Code Flow demonstration",
                RedirectUris =
                {
                    new Uri("https://www.thunderclient.com/oauth/callback"), // https://www.thunderclient.com/
                    new Uri($"{url}/swagger/oauth2-redirect.html"), // https://swagger.io/ for IdentityModule as Example
                    new Uri("https://localhost:20001/swagger/oauth2-redirect.html"), // https://swagger.io/ for Module as Example
                    new Uri("https://localhost:20002/swagger/oauth2-redirect.html"),
                    new Uri("https://localhost:7207/signin-oidc") // Calabonga.BlazorApp see folder ClientSamples in repository
                },
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7207/signout-callback-oidc") // Calabonga.BlazorApp see folder ClientSamples in repository
                },
                Permissions =
                {
                    // Endpoint permissions
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                    OpenIddictConstants.Permissions.Endpoints.Token,

                    // Grant type permissions
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                    // Scope permissions
                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "custom",
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,

                    // Response types
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.ResponseTypes.IdToken
                }
            }, cancellationToken);

            const string clientIdAngular = "client-id-angular";
            if (await manager.FindByClientIdAsync(clientIdAngular, cancellationToken) is null)
            {
                var angularUrl = "http://localhost:4200";

                await manager.CreateAsync(
                    new OpenIddictApplicationDescriptor
                    {
                        ClientId = clientIdAngular,
                        DisplayName = "Angular SPA Client",
                        ClientType = OpenIddictConstants.ClientTypes.Public,
                        ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                        Requirements = { OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange },
                        RedirectUris = { new Uri($"{angularUrl}/auth/callback") },
                        PostLogoutRedirectUris = { new Uri($"{angularUrl}/auth/signout-callback-oidc") },
                        Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            OpenIddictConstants.Permissions.ResponseTypes.Code,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                            OpenIddictConstants.Permissions.Endpoints.EndSession,
                            OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                        }
                    }, cancellationToken);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
