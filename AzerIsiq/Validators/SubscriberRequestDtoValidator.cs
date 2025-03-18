using AzerIsiq.Dtos;
using FluentValidation;

namespace AzerIsiq.Validators;

public class SubscriberRequestDtoValidator: AbstractValidator<SubscriberRequestDto>
{
    public SubscriberRequestDtoValidator() 
    {
        RuleFor(s => s.Name).NotEmpty().MaximumLength(25).WithMessage("Name is required");
        RuleFor(s => s.Surname).NotEmpty().MaximumLength(25).WithMessage("Surname is required");
        RuleFor(s => s.Patronymic).NotEmpty().MaximumLength(25).WithMessage("Father Name is required");
        RuleFor(s => s.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+994\d{9}$")
            .WithMessage("Incorrect phone number format.");
        RuleFor(s => s.FinCode)
            .NotEmpty().WithMessage("Fin Code is required.")
            .MinimumLength(7).WithMessage("Minimum fin code length is 7.")
            .MaximumLength(7).WithMessage("Maximum fin code length is 7.");
        RuleFor(s => s.PopulationStatus).NotEmpty().WithMessage("Population status is required.");
        // RuleFor(s => s.City).NotEmpty().MaximumLength(25).WithMessage("City name is required");
        // RuleFor(s => s.District).NotEmpty().MaximumLength(25).WithMessage("District is required");
        RuleFor(s => s.Building).NotEmpty().WithMessage("Building is required").MaximumLength(4).WithMessage("Maximum building length is 4.");
        RuleFor(s => s.Apartment).NotEmpty().WithMessage("Apartment is required").MaximumLength(4).WithMessage("Maximum apartment length is 4.");
    }
}