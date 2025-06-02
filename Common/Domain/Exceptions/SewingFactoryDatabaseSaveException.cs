namespace SewingFactory.Common.Domain.Exceptions;

public class SewingFactoryDatabaseSaveException : Exception
{
    public SewingFactoryDatabaseSaveException(string message)
        : base($"An error occurred while saving to the database: {message}")
    {
    }

    public SewingFactoryDatabaseSaveException(string message, Exception innerException)
        : base($"An error occurred while saving to the database: {message}", innerException)
    {
    }
}