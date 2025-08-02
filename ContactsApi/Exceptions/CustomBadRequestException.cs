namespace ContactsApi.Exceptions;

public class CustomBadRequestException : Exception
{
    public CustomBadRequestException(string message) : base(message)
    {}

    public CustomBadRequestException(string message, Exception innerException) : base(message, innerException)
    {}
}