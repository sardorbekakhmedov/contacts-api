using ContactsApi.Dtos;
using FluentValidation;

namespace ContactsApi.Validators;

public class PatchContactValidator : AbstractValidator<PatchContactDto>
{
    public PatchContactValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^998\d{9}$")
            .WithMessage("Phone number must start with 998, digits only — no '+' (e.g., 998901234567).");
    }
}