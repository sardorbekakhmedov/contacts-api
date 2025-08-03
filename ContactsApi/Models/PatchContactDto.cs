namespace ContactsApi.Models;

public class PatchContact
{
    public Guid Id { get; set; } 
    public required string PhoneNumber { get; set; }
}
