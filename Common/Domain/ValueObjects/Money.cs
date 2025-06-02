namespace SewingFactory.Common.Domain.ValueObjects;

public sealed class Money
{
    private decimal _amount;

    public Money(decimal amount)
    {
        Amount = amount;
    }

    public decimal Amount
    {
        get => _amount;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Amount of money cannot be negative");
            _amount = value;
        }
    }

    public static Money Zero => new(0);

    public static Money operator +(Money left, Money right)
    {
        return new Money(left.Amount + right.Amount);
    }

    public static Money operator *(Money money, int multiplier)
    {
        return new Money(money.Amount * multiplier);
    }

    public static Money operator *(int multiplier, Money money)
    {
        return new Money(money.Amount * multiplier);
    }

    public static Money operator *(Money money, decimal multiplier)
    {
        return new Money(money.Amount * multiplier);
    }

    public static Money operator *(decimal multiplier, Money money)
    {
        return new Money(money.Amount * multiplier);
    }
}