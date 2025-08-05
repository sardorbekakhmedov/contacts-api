using ContactsApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Data.Contexts;

public class ContactsDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ContactTag> ContactTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}