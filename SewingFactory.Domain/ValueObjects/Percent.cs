﻿namespace SewingFactory.Common.Domain.ValueObjects
{
    public class Percent
    {
        private  decimal _value;

        public Percent(decimal value)
        {
            Value = value;
        }

        public static implicit operator decimal(Percent percent) => percent.Value;

        public static implicit operator Percent(decimal value) => new(value);

        public decimal Value
        {
            get => _value;
            set
            {
                if (value is < 0 or > 100)
                    throw new ArgumentOutOfRangeException(nameof(value), "Percent must be between 0 and 100.");
                _value = value; 
            }
        }
    }
}
