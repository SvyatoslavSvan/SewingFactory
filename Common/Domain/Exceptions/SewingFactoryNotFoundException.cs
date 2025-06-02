namespace SewingFactory.Common.Domain.Exceptions;

public class SewingFactoryNotFoundException : Exception
{
    public SewingFactoryNotFoundException(string message)
        : base($"The requested item was not found: {message}")
    {
    }

    public SewingFactoryNotFoundException(string message, Exception innerException)
        : base($"The requested item was not found: {message}", innerException)
    {
    }
}