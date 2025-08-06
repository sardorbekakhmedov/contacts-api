using ContactsApi.Data.Contexts;
using ContactsApi.Entities;
using ContactsApi.Helper.Contacts;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Repositories;

public class ContactRepository(ContactsDbContext dbContext) : IContactRepository
{
    public async ValueTask<bool> ExistsPhoneNumberAsync(string phoneNumber, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.Contacts.AnyAsync(c =>
            c.PhoneNumber.Equals(phoneNumber, StringComparison.OrdinalIgnoreCase)
            && (excludeId == null || c.Id != excludeId), cancellationToken);
    }

    public async ValueTask<bool> ExistsEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.Contacts.AnyAsync(c =>
            c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
            && (excludeId == null || c.Id != excludeId), cancellationToken);
    }

    public async ValueTask<Contact?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Contacts.Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async ValueTask<List<Contact>> GetPagedAsync(ContactQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Contacts.Include(x => x.Tags).AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.FirstName))
            query = query.Where(c => c.FirstName.ToLower().Contains(queryParams.FirstName.ToLower()));

        if (!string.IsNullOrWhiteSpace(queryParams.LastName))
            query = query.Where(c => c.LastName.ToLower().Contains(queryParams.LastName.ToLower()));

        if (!string.IsNullOrWhiteSpace(queryParams.Email))
            query = query.Where(c => c.Email.ToLower().Contains(queryParams.Email.ToLower()));

        if (!string.IsNullOrWhiteSpace(queryParams.PhonNumber))
            query = query.Where(c => c.PhoneNumber.ToLower().Contains(queryParams.PhonNumber.ToLower()));

        if (!string.IsNullOrWhiteSpace(queryParams.Tag))
            query = query.Where(c => c.Tags.Any(t => t.Value == queryParams.Tag));

        if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
        {
            query = queryParams.SortBy.ToLower() switch
            {
                "firstname" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(c => c.FirstName)
                    : query.OrderBy(c => c.FirstName),
                "createdat" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(c => c.CreatedAt)
                    : query.OrderBy(c => c.CreatedAt),
                _ => query
            };
        }

        return await query
            .Skip(queryParams.Skip)
            .Take(queryParams.Take)
            .ToListAsync(cancellationToken);
    }

    public async ValueTask<int> CreateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        dbContext.Contacts.Add(contact);
        await dbContext.SaveChangesAsync(cancellationToken);
        return contact.Id;
    }

    public async ValueTask<Contact> UpdateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        dbContext.Contacts.Update(contact);
        await dbContext.SaveChangesAsync(cancellationToken);
        return contact;
    }

    public async ValueTask<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await dbContext.Contacts
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result;
    }
}
