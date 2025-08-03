using AutoMapper;
using ContactsApi.Dtos;
using ContactsApi.Entities;
using ContactsApi.Exceptions;
using ContactsApi.Helper.Contacts;
using ContactsApi.Models;

namespace ContactsApi.Services.Contacts;

public class ContactService(IMapper mapper) : IContactService
{
    public static List<Contact> contacts = new();

    public async ValueTask<bool> ExistsPhoneNumberAsync(string phoneNumber, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        
        if (excludeId is not null)
            return contacts.Any(c => c.PhoneNumber.Equals(phoneNumber, StringComparison.OrdinalIgnoreCase) && c.Id != excludeId);

        return contacts.Any(c => c.PhoneNumber.Equals(phoneNumber, StringComparison.OrdinalIgnoreCase));
    }

    public async ValueTask<bool> ExistsEmailAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        if(excludeId is not null)
            return contacts.Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && c.Id != excludeId);

        return contacts.Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async ValueTask<Guid> CreateAsync(CreateContact model, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        if (await ExistsEmailAsync(model.Email, null, cancellationToken))
            throw new CustomConflictException($"Email '{model.Email}' is already in use.");

        if (await ExistsPhoneNumberAsync(model.PhoneNumber, null, cancellationToken))
            throw new CustomConflictException($"Phone number '{model.PhoneNumber}' is already in use.");

        var newContact = mapper.Map<Contact>(model);
        newContact.Id = Guid.NewGuid();
        newContact.CreateAt = DateTime.Now;

        contacts.Add(newContact);

        return newContact.Id;
    }

    public async ValueTask<IEnumerable<ViewContact>> GetAllAsync(ContactQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        if (queryParams.Take > 100)
            throw new CustomBadRequestException("Max number of results (take) cannot exceed 100.");

        var query = contacts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.FirstName))
            query = query.Where(c => c.FirstName.ToLower().Contains(queryParams.FirstName.ToLower(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(queryParams.LastName))
            query = query.Where(c => c.LastName.ToLower().Contains(queryParams.LastName.ToLower(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(queryParams.Email))
            query = query.Where(c => c.Email.ToLower().Contains(queryParams.Email.ToLower(), StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(queryParams.PhonNumber))
            query = query.Where(c => c.PhoneNumber.ToLower().Contains(queryParams.PhonNumber.ToLower(), StringComparison.OrdinalIgnoreCase));
            
        if (!string.IsNullOrWhiteSpace(queryParams.Tag))
        {
            query = query.Where(c => c.Tags.Contains(queryParams.Tag, StringComparer.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(queryParams.SortBy))
        {
            query = queryParams.SortBy.ToLower() switch
            {
                "firstname" => queryParams.SortOrder == "desc"
                                ? query.OrderByDescending(c => c.FirstName)
                                : query.OrderBy(c => c.FirstName),
                "createdat" => queryParams.SortOrder == "desc"
                                ? query.OrderByDescending(c => c.CreateAt)
                                : query.OrderBy(c => c.CreateAt),
                _ => query
            };
        }

        var pagedContacts = query
            .Skip(queryParams.Skip)
            .Take(queryParams.Take)
            .ToList();

        return mapper.Map<List<ViewContact>>(pagedContacts);
    }

    public async ValueTask<ViewContact> GetSingleOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        var contact = contacts.SingleOrDefault(c => c.Id == id);

        return mapper.Map<ViewContact>(contact);
    }

    public async ValueTask<ViewContact> GetSingleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await GetSingleOrDefaultAsync(id, cancellationToken) ??
            throw new CustomNotFoundException($"{nameof(Contact)} with id '{id}' not found.");

        return mapper.Map<ViewContact>(contact);
    }

    public async ValueTask<ViewContact> UpdateAsync(Guid id, UpdateContact model, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        var contactToUpdate = contacts.SingleOrDefault(c => c.Id == id) ??
            throw new CustomNotFoundException($"{nameof(Contact)} with id '{id}' not found."); 

        mapper.Map(model, contactToUpdate);
        contactToUpdate.UpdatedAt = DateTime.Now;

        return mapper.Map<ViewContact>(contactToUpdate);
    }

    public async ValueTask<ViewContact> UpdatePhoneNumberAsync(Guid id, PatchContact model, CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        var contactToUpdate = contacts.SingleOrDefault(c => c.Id == id) ??
            throw new CustomNotFoundException($"{nameof(Contact)} with id '{id}' not found.");

        contactToUpdate.PhoneNumber = model.PhoneNumber;
        contactToUpdate.UpdatedAt = DateTime.Now;
        return mapper.Map<ViewContact>(contactToUpdate);
    }
    
    public async ValueTask DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        
        var contactToDelete = contacts.SingleOrDefault(c => c.Id == id) ??
            throw new CustomNotFoundException($"{nameof(Contact)} with id '{id}' not found.");

        contacts.Remove(contactToDelete);
    }
}