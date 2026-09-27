using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.ResetPassword
{
    public class ResetPasswordCommandValidator
      : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("You must enter new password");

            RuleFor(x => x.ResetToken)
                .NotEmpty()
                .WithMessage("Reset token is required");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User id is required");
        }
    }
}
