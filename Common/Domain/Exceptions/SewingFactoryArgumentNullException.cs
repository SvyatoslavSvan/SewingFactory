namespace SewingFactory.Common.Domain.Exceptions;

public class SewingFactoryArgumentNullException : ArgumentNullException
{
    public SewingFactoryArgumentNullException(string argumentName)
        : base($"The {argumentName} is NULL")
    {
    }

    public SewingFactoryArgumentNullException(string argumentName, Exception? exception)
        : base($"The {argumentName} is NULL", exception)
    {
    }
}