namespace SewingFactory.Common.Domain.Exceptions;

public class SewingFactoryAccessDeniedException : Exception
{
    public SewingFactoryAccessDeniedException(string message)
        : base($"Access to the BizSuite is denied: {message}")
    {
    }

    public SewingFactoryAccessDeniedException(string message, Exception innerException)
        : base($"Access to the BizSuite is denied: {message}", innerException)
    {
    }
}