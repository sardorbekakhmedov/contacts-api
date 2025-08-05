using ContactsApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Data.Contexts;

public class ContactsContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ContactTag> ContactTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactsContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}