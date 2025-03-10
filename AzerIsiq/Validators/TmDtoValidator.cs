using AzerIsiq.Dtos;
using FluentValidation;

namespace AzerIsiq.Validators;


public class TmDtoValidator : AbstractValidator<TmDto>
{
    public TmDtoValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.SubstationId)
                .GreaterThan(0)
                .WithMessage("SubstationId must be greater than 0");
        });

        RuleSet("Update", () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .When(x => x.Name != null)
                .WithMessage("Name is required");

            RuleFor(x => x.SubstationId)
                .GreaterThan(0)
                .When(x => x.SubstationId != null)
                .WithMessage("SubstationId must be greater than 0");
        });
    }
}
