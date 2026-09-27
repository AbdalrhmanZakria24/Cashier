using FluentValidation;

namespace Cashier.Application.CQRS.Command.Identity.Register
{
    public class RegisterCommandValidator
        : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Please enter your name");

            RuleFor(x => x.email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please enter a valid email");

            RuleFor(x => x.phoneNumber)
                .NotEmpty()
                .WithMessage("Please enter your phone number");

            RuleFor(x => x.password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage(
                    "Password must be at least 6 characters");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.password)
                .WithMessage(
                    "Password confirmation does not match");
        }
    }
}