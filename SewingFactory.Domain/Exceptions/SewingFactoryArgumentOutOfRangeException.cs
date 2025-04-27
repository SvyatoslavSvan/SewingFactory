namespace SewingFactory.Common.Domain.Exceptions;

public class SewingFactoryArgumentOutOfRangeException : ArgumentOutOfRangeException
{
    public SewingFactoryArgumentOutOfRangeException(string argumentName)
        : base(argumentName, $"The argument '{argumentName}' is out of range.")
    {
    }


    public SewingFactoryArgumentOutOfRangeException(string argumentName, string? details)
        : base(argumentName, $"The argument '{argumentName}' is out of range. Details: {details}")
    {
    }
}