namespace ContactsApi.Dtos;

public class PatchContactDto
{
    public int Id { get; set; }
    public required string PhoneNumber { get; set; }
}
