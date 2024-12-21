namespace SewingFactory.Common.Domain.Exceptions
{
    public class BizSuiteNotFoundException : Exception
    {
        public BizSuiteNotFoundException(string message)
            : base($"The requested item was not found: {message}") { }

        public BizSuiteNotFoundException(string message, Exception innerException)
            : base($"The requested item was not found: {message}", innerException) { }
    }
}
