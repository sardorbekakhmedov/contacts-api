namespace ContactsApi.Models;

public class CreateContact
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public List<string>? Tags { get; set; } 
}
