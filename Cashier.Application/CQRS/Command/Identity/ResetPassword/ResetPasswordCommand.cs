using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.ResetPassword
{
    public record ResetPasswordCommand(
       string Password,
       string ResetToken,
       string UserId
   ) : IRequest<ResultT<string>>;
}
