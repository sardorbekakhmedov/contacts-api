using AutoMapper;
using ContactsApi.Dtos;
using ContactsApi.Entities;
using ContactsApi.Exceptions;
using ContactsApi.Helper.Contacts;
using ContactsApi.Models;

namespace ContactsApi.Services.Contacts;

public interface IContactService
{
    ValueTask<int> CreateAsync(CreateContact model, CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<ViewContact>> GetAllAsync(ContactQueryParams queryParams, CancellationToken cancellationToken = default);
    ValueTask<ViewContact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<ViewContact> GetSingleAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<ViewContact> UpdateAsync(int id, UpdateContact model, CancellationToken cancellationToken = default);
    ValueTask<ViewContact> UpdatePhoneNumberAsync(int id, PatchContact model, CancellationToken cancellationToken = default);

    ValueTask<bool> ExistsPhoneNumberAsync(string phoneNumber, int? excludeId = null,
        CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default);
}