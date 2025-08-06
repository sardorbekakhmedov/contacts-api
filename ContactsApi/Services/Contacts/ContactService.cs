using AutoMapper;
using ContactsApi.Entities;
using ContactsApi.Exceptions;
using ContactsApi.Helper.Contacts;
using ContactsApi.Models;
using ContactsApi.Repositories;

namespace ContactsApi.Services.Contacts;

public class ContactService(
    IContactRepository repository,
    IMapper mapper) : IContactService
{
    public async ValueTask<int> CreateAsync(CreateContact model, CancellationToken cancellationToken = default)
    {
        if (await repository.ExistsEmailAsync(model.Email, null, cancellationToken))
            throw new CustomConflictException($"Email '{model.Email}' is already in use.");

        if (await repository.ExistsPhoneNumberAsync(model.PhoneNumber, null, cancellationToken))
            throw new CustomConflictException($"Phone number '{model.PhoneNumber}' is already in use.");

        var contact = mapper.Map<Contact>(model);
        contact.CreatedAt = DateTimeOffset.Now;

        return await repository.CreateAsync(contact, cancellationToken);
    }

    public async ValueTask<IEnumerable<ViewContact>> GetAllAsync(ContactQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        if (queryParams.Take > 100)
            throw new CustomBadRequestException("Max number of results (take) cannot exceed 100.");

        var contacts = await repository.GetPagedAsync(queryParams, cancellationToken);
        return mapper.Map<List<ViewContact>>(contacts);
    }

    public async ValueTask<ViewContact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, cancellationToken);
        return contact is null ? null : mapper.Map<ViewContact>(contact);
    }
    

    public async ValueTask<ViewContact> GetSingleAsync(int id, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, cancellationToken) ??
            throw new CustomNotFoundException($"Contact with id '{id}' not found.");

        return mapper.Map<ViewContact>(contact);
    }

    public async ValueTask<ViewContact> UpdateAsync(int id, UpdateContact model, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, cancellationToken) ??
            throw new CustomNotFoundException($"Contact with id '{id}' not found.");

        mapper.Map(model, contact);
        contact.UpdatedAt = DateTimeOffset.Now;

        var updated = await repository.UpdateAsync(contact, cancellationToken);
        return mapper.Map<ViewContact>(updated);
    }

    public async ValueTask<ViewContact> UpdatePhoneNumberAsync(int id, PatchContact model, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, cancellationToken) ??
            throw new CustomNotFoundException($"Contact with id '{id}' not found.");

        contact.PhoneNumber = model.PhoneNumber;
        contact.UpdatedAt = DateTimeOffset.Now;

        var updated = await repository.UpdateAsync(contact, cancellationToken);
        return mapper.Map<ViewContact>(updated);
    }

    public async ValueTask<bool> ExistsPhoneNumberAsync(string phoneNumber, int? excludeId = null, CancellationToken cancellationToken = default)
        => await repository.ExistsPhoneNumberAsync(phoneNumber, excludeId, cancellationToken);
    

    public async ValueTask<bool> ExistsEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default) 
        => await repository.ExistsEmailAsync(email, excludeId, cancellationToken);

    
    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(id, cancellationToken);
        if (result == 0)
            throw new CustomNotFoundException($"Contact with id '{id}' not found.");
    }
}
