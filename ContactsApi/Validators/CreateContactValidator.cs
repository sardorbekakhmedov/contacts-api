using ContactsApi.Dtos;
using FluentValidation;

namespace ContactsApi.Validators;

public class CreateContactValidator : AbstractValidator<CreateContactDto>
{
    public CreateContactValidator()
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(contac => contac.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(1)
            .WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters.");

        RuleFor(contact => contact.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(1)
            .WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters.");

        RuleFor(contact => contact.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^998\d{9}$")
            .WithMessage("Phone number must start with 998, digits only — no '+' (e.g., 998901234567).");

        RuleFor(contact => contact.Address)
            .MaximumLength(200)
            .WithMessage("Address cannot exceed 200 characters.")
            .When(contact => string.IsNullOrWhiteSpace(contact.Address) is false);
    }
}