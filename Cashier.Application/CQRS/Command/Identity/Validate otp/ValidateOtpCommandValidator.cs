using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.Validate_otp
{
    public class ValidateOtpCommandValidator: AbstractValidator<ValidateOtpCommand>
    {
        public ValidateOtpCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User id is required");

            RuleFor(x => x.Otp)
                .NotEmpty()
                .WithMessage("OTP is required")
                .Length(6)
                .WithMessage("OTP must be 6 digits")
                .Matches(@"^\d{6}$")
                .WithMessage("OTP must contain only numbers");
        }
    }
}
