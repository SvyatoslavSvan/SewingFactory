﻿namespace SewingFactory.Common.Domain.Base;

/// <summary>
///     Static data container
/// </summary>
public static partial class AppData
{
    /// <summary>
    ///     CORS Policy name
    /// </summary>
    public const string PolicyName = "CorsPolicy";

    /// <summary>
    ///     Current service name
    /// </summary>
    public const string ServiceName = "Microservice Template";

    /// <summary>
    ///     Nimble Framework Microservice Template with integrated OpenIddict for OpenID Connect server and Token Validation
    /// </summary>
    public const string ServiceDescription =
        "Nimble Framework Microservice Template with integrated OpenIddict for OpenID Connect server and Token Validation";

    /// <summary>
    ///     "SystemAdministrator"
    /// </summary>
    public const string SystemAdministratorRoleName = "Administrator";

    /// <summary>
    ///     "BusinessOwner"
    /// </summary>
    public const string ManagerRoleName = "Manager";


    /// <summary>
    ///     Default policy name for CORS
    /// </summary>
    public const string PolicyCorsName = "CorsPolicy";

    /// <summary>
    ///     Default policy name for API
    /// </summary>
    public const string PolicyDefaultName = "DefaultPolicy";

    /// <summary>
    ///     Roles
    /// </summary>
    public static IEnumerable<string> Roles
    {
        get
        {
            yield return SystemAdministratorRoleName;
            yield return ManagerRoleName;
        }
    }
}