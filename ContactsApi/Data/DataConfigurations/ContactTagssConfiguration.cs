using ContactsApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsApi.Data.DataConfigurations;

public class ContactTagsConfiguration : IEntityTypeConfiguration<ContactTag>
{
    public void Configure(EntityTypeBuilder<ContactTag> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Value).HasMaxLength(20).IsRequired();;
    }
}