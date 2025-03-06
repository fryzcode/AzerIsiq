using AzerIsiq.Dtos;
using FluentValidation;

namespace AzerIsiq.Validators;

public class ResetUserValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetUserValidator()
    {
        RuleFor(p => p.NewPassword)
            .NotNull().WithMessage("The password confirmation field is required")
            .Equal(p => p.ConfirmNewPassword)
            .WithMessage("Password and Password Confirmation don't match");
    }
}