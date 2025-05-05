using AzerIsiq.Dtos.ElectronicAppealDto;
using FluentValidation;

namespace AzerIsiq.Validators;

public class ElectronicAppealCreateDtoValidator : AbstractValidator<ElectronicAppealCreateDto>
{
    public ElectronicAppealCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage("Name is required");;
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(50).WithMessage("Surname is required");;
        RuleFor(s => s.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+994\d{9}$")
            .WithMessage("Incorrect phone number format.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");;
        RuleFor(x => x.Content).NotEmpty().MinimumLength(10).MaximumLength(250).WithMessage("Content is required");
    }
}