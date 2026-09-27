using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.ForgetPassword
{
    public sealed record ForgetPasswordCommand(
         string Email
     ) : IRequest<ResultT<bool>>;
}
