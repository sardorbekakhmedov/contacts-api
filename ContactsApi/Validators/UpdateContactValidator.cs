using ContactsApi.Dtos;
using ContactsApi.Services.Contacts;
using FluentValidation;

namespace ContactsApi.Validators;

public class UpdateContactValidator : AbstractValidator<UpdateContactDto>
{
    public UpdateContactValidator(IContactService contactService)
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(contac => contac.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters.");

        RuleFor(contact => contact.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters.");

        RuleFor(contact => contact.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters.")
            .MustAsync(async (model, email, cancellationToken) 
                => !await contactService.ExistsEmailAsync(email, model.Id, cancellationToken))
            .WithMessage($"This email is already in use.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Length(12).WithMessage("Phone number must be exactly 12 digits long.")
            .Matches(@"^998\d{9}$")
            .WithMessage("Phone number must start with 998, digits only — no '+' (e.g., 998901234567).")
            .MustAsync(async (model, phoneNumber, cancellationToken) 
                => !await contactService.ExistsPhoneNumberAsync(phoneNumber, model.Id, cancellationToken))
            .WithMessage($"This phoneNumber is already in use.");;

        RuleFor(contact => contact.Address)
            .MinimumLength(5)
            .WithMessage("Address must be at least 5 characters long.")
            .MaximumLength(200)
            .WithMessage("Address must not exceed 220 characters.")
            .When(contact => string.IsNullOrWhiteSpace(contact.Address) is false);
    }
}