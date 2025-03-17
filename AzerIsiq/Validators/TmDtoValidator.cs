using AzerIsiq.Dtos;
using FluentValidation;

namespace AzerIsiq.Validators;
public class TmDtoValidator : AbstractValidator<TmDto>
{
    public TmDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Matches(@"^tm-.*$").WithMessage("Transformator Name must start with 'tm-' if it's provided.");

        RuleFor(x => x.SubstationId)
            .GreaterThan(0)
            .WithMessage("Uncorrect Substation");
    }
}
