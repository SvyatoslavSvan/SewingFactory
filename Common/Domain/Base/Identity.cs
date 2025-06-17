namespace SewingFactory.Common.Domain.Base;

/// <summary>
///     Identifier
/// </summary>
public abstract class Identity : IHaveId
{
    protected Identity()
    {
        
    }

    protected Identity(Guid id)
    {
        Id = id;
    }
    
    /// <summary>
    ///     Identifier
    /// </summary>
    public Guid Id { get; }
}