namespace SewingFactory.Common.Domain.Exceptions;

public class BizSuiteInvalidOperationException : InvalidOperationException
{
    public BizSuiteInvalidOperationException(string message)
        : base($"The operation is invalid: {message}") { }

    public BizSuiteInvalidOperationException(string message, Exception innerException)
        : base($"The operation is invalid: {message}", innerException) { }
}