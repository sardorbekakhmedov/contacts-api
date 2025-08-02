namespace ContactsApi.Exceptions;

public class CustomConflictException : Exception
{
    public CustomConflictException(string message) : base(message)
    {}

    public CustomConflictException(string message, Exception innerException) : base(message, innerException)
    {}
}