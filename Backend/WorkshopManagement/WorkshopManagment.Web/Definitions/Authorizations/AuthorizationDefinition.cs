﻿using Calabonga.AspNetCore.AppDefinitions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server.AspNetCore;
using SewingFactory.Backend.WorkshopManagement.Web.Definitions.OpenIddict;
using SewingFactory.Common.Domain.Base;
using System.Text.Json;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.Authorizations;

/// <summary>
///     Authorization Policy registration
/// </summary>
public class AuthorizationDefinition : AppDefinition
{
    /// <summary>
    ///     Configure services for current microservice
    /// </summary>
    /// <param name="builder"></param>
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var url = builder.Configuration.GetSection("AuthServer").GetValue<string>("Url");

        builder.Services
            .AddAuthentication(configureOptions: options =>
            {
                options.DefaultScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, "Bearer", configureOptions: options =>
            {
                options.SaveToken = true;
                options.Audience = "client-id-code";
                options.Authority = url;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, // Audience should be defined on the authorization server or disabled as shown
                    ClockSkew = new TimeSpan(0, 0, 30)
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Ensure we always have an error and error description.
                        if (string.IsNullOrEmpty(context.Error))
                        {
                            context.Error = "invalid_token";
                        }

                        if (string.IsNullOrEmpty(context.ErrorDescription))
                        {
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";
                        }

                        // Add some extra context for expired tokens.
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                            context.Response.Headers.Append("x-token-expired", authenticationException?.Expires.ToString("o"));
                            context.ErrorDescription = $"The token expired on {authenticationException?.Expires:o}";
                        }

                        return context.Response.WriteAsync(JsonSerializer.Serialize(new { error = context.Error, error_description = context.ErrorDescription }));
                    }
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(AuthData.AuthSchemes)
                .Build();

            options.DefaultPolicy = policy;
            options.FallbackPolicy = policy;

            options.AddPolicy(AppData.DesignerAccess,
                p => p.Requirements.Add(new PermissionRequirement(AppData.DesignerAccess)));

            options.AddPolicy(AppData.FinanceAccess,
                p => p.Requirements.Add(new PermissionRequirement(AppData.FinanceAccess)));
        });

        builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        builder.Services.AddSingleton<IAuthorizationHandler, AppPermissionHandler>();
    }

    /// <summary>
    ///     Configure application for current microservice
    /// </summary>
    /// <param name="app"></param>
    public override void ConfigureApplication(WebApplication app)
    {
        app.UseRouting();
        app.UseCors(AppData.PolicyName);
        app.UseAuthentication();
        app.UseAuthorization();

        // registering UserIdentity helper as singleton
        UserIdentity.Instance.Configure(app.Services.GetService<IHttpContextAccessor>()!);
    }
}