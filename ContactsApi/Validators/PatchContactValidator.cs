using ContactsApi.Dtos;
using ContactsApi.Services.Contacts;
using FluentValidation;

namespace ContactsApi.Validators;

public class PatchContactValidator : AbstractValidator<PatchContactDto>
{
    public PatchContactValidator(IContactService contactService)
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Length(12).WithMessage("Phone number must be exactly 12 digits long.")
            .Matches(@"^998\d{9}$")
            .WithMessage("Phone number must start with 998, digits only — no '+' (e.g., 998901234567).")
            .MustAsync(async (model, phoneNumber, cancellationToken) =>
            {
                return !await contactService.ExistsPhoneNumberAsync(phoneNumber, model.Id, cancellationToken);
            })
            .WithMessage($"This phoneNumber is already in use.");;
    }
}