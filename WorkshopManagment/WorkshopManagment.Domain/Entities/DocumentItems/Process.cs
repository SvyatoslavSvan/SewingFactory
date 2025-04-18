using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;

public sealed class Process : NamedIdentity
{
    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private Process() { }

    public Process(string name, Money? price = null) : base(name) => Price = price ?? new Money(0);

    public Money Price { get; set; } = null!;
}