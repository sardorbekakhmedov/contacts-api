namespace ContactsApi.Dtos;

public class PatchContactDto
{
    public Guid Id { get; set; }
    public required string PhoneNumber { get; set; }
}
