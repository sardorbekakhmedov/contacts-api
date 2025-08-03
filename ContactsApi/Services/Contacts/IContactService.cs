using AutoMapper;
using ContactsApi.Dtos;
using ContactsApi.Entities;
using ContactsApi.Exceptions;
using ContactsApi.Helper.Contacts;
using ContactsApi.Models;

namespace ContactsApi.Services.Contacts;

public interface IContactService
{
    ValueTask<Guid> CreateAsync(CreateContact model, CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<ViewContact>> GetAllAsync(ContactQueryParams queryParams, CancellationToken cancellationToken = default);
    ValueTask<ViewContact> GetSingleOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);
    ValueTask<ViewContact> GetSingleAsync(Guid id, CancellationToken cancellationToken = default);
    ValueTask<ViewContact> UpdateAsync(Guid id, UpdateContact model, CancellationToken cancellationToken = default);
    ValueTask<ViewContact> UpdatePhoneNumberAsync(Guid id, PatchContact model, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsEmailAsync(string email, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}