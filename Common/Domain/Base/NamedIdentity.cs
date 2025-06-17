using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Common.Domain.Base;

/// <summary>
///     NamedIdentity dictionary for selector
/// </summary>
public abstract class NamedIdentity : Identity
{
    private string _name = null!;


    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    protected NamedIdentity()
    {
    }

    protected NamedIdentity(string name)
    {
        Name = name;
    }
    
    protected NamedIdentity(string name, Guid id) : base(id)
    {
        Name = name;
    }

    /// <summary>
    ///     Entity name
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new SewingFactoryArgumentNullException(nameof(value));
            _name = value;
        }
    }
}