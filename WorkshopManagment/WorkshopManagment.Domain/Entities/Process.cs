using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities;

public sealed class Process(string name, Money? price = null) : NamedIdentity(name)
{
    public Money Price { get; set; } = price ?? new Money(0);
}