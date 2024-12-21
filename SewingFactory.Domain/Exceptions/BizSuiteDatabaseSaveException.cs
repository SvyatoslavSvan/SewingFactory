namespace SewingFactory.Common.Domain.Exceptions;

public class BizSuiteDatabaseSaveException : Exception
{
    public BizSuiteDatabaseSaveException(string message)
        : base($"An error occurred while saving to the database: {message}") { }

    public BizSuiteDatabaseSaveException(string message, Exception innerException)
        : base($"An error occurred while saving to the database: {message}", innerException) { }
}