using ContactsApi.Helper.Contacts;

namespace ContactsApi.Repositories;
using ContactsApi.Entities;

public interface IContactRepository
{
    ValueTask<bool> ExistsPhoneNumberAsync(string phoneNumber, int? excludeId = null, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
    ValueTask<Contact?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<List<Contact>> GetPagedAsync(ContactQueryParams queryParams, CancellationToken cancellationToken = default);
    ValueTask<int> CreateAsync(Contact contact, CancellationToken cancellationToken = default);
    ValueTask<Contact> UpdateAsync(Contact contact, CancellationToken cancellationToken = default);
    ValueTask<int> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

