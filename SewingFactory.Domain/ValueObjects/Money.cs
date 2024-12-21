namespace SewingFactory.Common.Domain.ValueObjects
{
    public class Money
    {
        private decimal _amount;

        public Money(decimal amount) => Amount = amount;

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Amount of money cannot be negative");
                }
                _amount = value;
            }
        }

        public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);

        public static Money operator *(Money money, int multiplier) => new(money.Amount * multiplier);

        public static Money operator *(int multiplier, Money money) => new(money.Amount * multiplier);
    }
}
