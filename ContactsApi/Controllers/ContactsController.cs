using AutoMapper;
using ContactsApi.Dtos;
using ContactsApi.Helper.Contacts;
using ContactsApi.Models;
using ContactsApi.Services.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(
    IMapper mapper,
    IContactService contactService) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> GetAll([FromQuery] ContactQueryParams queryParams, CancellationToken cancellationToken)
    {
        var result = await contactService.GetAllAsync(queryParams, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var contact = await contactService.GetSingleAsync(id, cancellationToken);
        return Ok(contact);
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create([FromBody] CreateContactDto dto, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateContact>(dto);

        var newId = await contactService.CreateAsync(model, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Update(Guid id, [FromBody] UpdateContactDto dto, CancellationToken cancellationToken)
    {
        var model = mapper.Map<UpdateContact>(dto);
        var updated = await contactService.UpdateAsync(id, model, cancellationToken);
        return Ok(updated);
    }

    [HttpPatch("{id:guid}/phone-number")]
    public async ValueTask<IActionResult> UpdatePhoneNumber(Guid id, [FromBody] PatchContactDto dto, CancellationToken cancellationToken)
    {
        var model = mapper.Map<PatchContact>(dto);
        var updated = await contactService.UpdatePhoneNumberAsync(id, model, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await contactService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
