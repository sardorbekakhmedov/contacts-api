namespace ContactsApi.Models;

public class PatchContact
{
    public int Id { get; set; } 
    public required string PhoneNumber { get; set; }
}
