namespace SewingFactory.Common.Domain.Exceptions;

public class SewingFactoryInvalidOperationException : InvalidOperationException
{
    public SewingFactoryInvalidOperationException(string message)
        : base($"The operation is invalid: {message}") { }

    public SewingFactoryInvalidOperationException(string message, Exception innerException)
        : base($"The operation is invalid: {message}", innerException) { }
}