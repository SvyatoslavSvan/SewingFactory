namespace SewingFactory.Common.Domain.Exceptions;

public class BizSuiteArgumentNullException : ArgumentNullException
{
    public BizSuiteArgumentNullException(string argumentName)
        : base($"The {argumentName} is NULL") { }

    public BizSuiteArgumentNullException(string argumentName, Exception? exception)
        : base($"The {argumentName} is NULL", exception) { }
}