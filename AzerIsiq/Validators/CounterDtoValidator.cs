using AzerIsiq.Dtos;
using FluentValidation;

namespace AzerIsiq.Validators;

public class CounterDtoValidator : AbstractValidator<CounterDto>
{
    
    public CounterDtoValidator()
    {
        RuleFor(x => x.Coefficient)
            .Must(x => new[] { 50, 100, 200 }.Contains(x))
            .WithMessage("The coefficient should be 50, 100 or 200");

        RuleFor(x => x.Phase)
            .Must(phase => phase == 1 || phase == 3)
            .WithMessage("Phase can only be 1 or 3");
    }
}