using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Common.Domain.ValueObjects;

public sealed class Percent
{
    private decimal _value;

    public Percent(decimal value)
    {
        Value = value;
    }

    public decimal Value
    {
        get => _value;
        set
        {
            if (value is < 0 or > 100)
                throw new SewingFactoryArgumentOutOfRangeException(nameof(value), "Percent must be between 0 and 100.");
            _value = value;
        }
    }

    public static implicit operator decimal(Percent percent)
    {
        return percent.Value;
    }

    public static implicit operator Percent(decimal value)
    {
        return new Percent(value);
    }

    public decimal ToDecimal()
    {
        return Value / 100;
    }
}