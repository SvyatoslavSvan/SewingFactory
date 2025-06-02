namespace SewingFactory.Common.Domain.Exceptions;

public class SewingFactoryArgumentException : ArgumentException
{
    public SewingFactoryArgumentException(string argumentName)
        : base($"The argument '{argumentName}' is invalid.")
    {
    }

    public SewingFactoryArgumentException(string argumentName, string? details)
        : base($"The argument '{argumentName}' is invalid. Details: {details}")
    {
    }
}