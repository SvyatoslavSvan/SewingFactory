namespace SewingFactory.Common.Domain.Exceptions;

public class BizSuiteAccessDeniedException : Exception
{
    public BizSuiteAccessDeniedException(string message)
        : base($"Access to the BizSuite is denied: {message}") { }

    public BizSuiteAccessDeniedException(string message, Exception innerException)
        : base($"Access to the BizSuite is denied: {message}", innerException) { }
}