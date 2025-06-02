namespace SewingFactory.Common.Domain.Base;

/// <summary>
///     Order
/// </summary>
public class Sortable(DateTime createdAt, string createdBy, int sortIndex) : Auditable(createdAt, createdBy)
{
    /// <summary>
    ///     Sorting index for entity
    /// </summary>
    public int SortIndex { get; set; } = sortIndex;
}