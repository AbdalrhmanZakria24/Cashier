using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.Validate_otp
{
    public sealed record ValidateOtpCommand(
        string UserId,
        string Otp
    ) : IRequest<ResultT<string>>;
}
