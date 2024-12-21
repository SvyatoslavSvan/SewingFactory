namespace SewingFactory.Common.Domain.Exceptions
{
    public class BizSuiteArgumentException : ArgumentException
    {
        public BizSuiteArgumentException(string argumentName)
            : base($"The argument '{argumentName}' is invalid.") { }

        public BizSuiteArgumentException(string argumentName, string? details)
            : base($"The argument '{argumentName}' is invalid. Details: {details}") { }
    }

}
