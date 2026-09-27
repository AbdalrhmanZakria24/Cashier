using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.ForgetPassword
{
    public class ForgetPasswordCommandValidator
        : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Enter your email or username");
        }
    }
}
