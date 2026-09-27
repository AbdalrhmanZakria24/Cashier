namespace Cashier.Application.CQRS.Command.Identity.Login
{
    public class LoginCommandValidator:AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please enter your email");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Please enter your Password");
        }
    }
}
