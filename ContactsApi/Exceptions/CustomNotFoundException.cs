namespace ContactsApi.Exceptions;

public class CustomNotFoundException : Exception
{
    public CustomNotFoundException(string message) : base(message)
    { }

    public CustomNotFoundException(string message, Exception innerException) : base(message, innerException)
    { }
}
