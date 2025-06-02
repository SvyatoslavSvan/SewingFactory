namespace SewingFactory.Common.Domain.Base;

/// <summary>
///     Represent information about creation and last update
/// </summary>
public interface IAuditable
{
    /// <summary>
    ///     DateTime of creation. This value will never changed
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    ///     Author name. This value never changed
    /// </summary>
    string CreatedBy { get; }

    /// <summary>
    ///     DateTime of last value update. Should be updated when entity data updated
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    ///     Author of last value update. Should be updated when entity data updated
    /// </summary>
    string? UpdatedBy { get; }
}