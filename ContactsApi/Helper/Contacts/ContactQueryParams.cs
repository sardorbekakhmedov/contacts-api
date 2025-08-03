namespace ContactsApi.Helper.Contacts;

public class ContactQueryParams
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhonNumber { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
