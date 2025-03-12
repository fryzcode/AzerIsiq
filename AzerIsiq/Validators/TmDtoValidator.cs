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
                .NotEmpty().WithMessage("Name is required.")
                .Matches(@"^tm-.*$").WithMessage("Transformator Name must start with 'tm-' if it's provided.");

            RuleFor(x => x.SubstationId)
                .GreaterThan(0)
                .WithMessage("SubstationId must be greater than 0");
        });

        RuleSet("Update", () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().NotEmpty().WithMessage("Name is required.")
                .Matches(@"^tm-.*$").WithMessage("Transformator Name must start with 'tm-' if it's provided.")
                .When(x => x.Name != null);

            RuleFor(x => x.SubstationId)
                .GreaterThan(0)
                .When(x => x.SubstationId != null)
                .WithMessage("SubstationId must be greater than 0");
        });
    }
}
