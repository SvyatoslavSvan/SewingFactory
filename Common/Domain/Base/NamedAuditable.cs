namespace SewingFactory.Common.Domain.Base;

/// <summary>
///     Audit-able with name
/// </summary>
public abstract class NamedAuditable : Auditable
{
    protected NamedAuditable(DateTime createdAt, string createdBy, string name) : base(createdAt, createdBy)
    {
        Name = name;
    }

    public string Name { get; protected set; } = null!;
}