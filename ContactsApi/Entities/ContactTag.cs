namespace ContactsApi.Entities;

public class ContactTag
{
    public int Id { get; set; }
    public required string Value { get; set; }

    public int ContactId { get; set; }
    public Contact? Contact { get; set; }
}
