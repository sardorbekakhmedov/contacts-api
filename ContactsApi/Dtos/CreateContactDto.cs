namespace ContactsApi.Dtos;

public class CreateContactDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public List<CreateOrUpdateContactTagDto>? Tags { get; set; } 
}
