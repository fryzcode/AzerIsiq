using AzerIsiq.Dtos;
using FluentValidation;

namespace AzerIsiq.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(25).WithMessage("UserName is required");
        RuleFor(m=>m.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        RuleFor(p => p.PasswordConfirmation)
            .NotNull().WithMessage("The password confirmation field is required")
            .Equal(p => p.Password)
            .WithMessage("Password and Password Confirmation don't match");
    }
}