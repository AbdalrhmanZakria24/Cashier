using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Identity.RefreshToken
{
    public sealed record RefreshTokenCommand(string AccessToken,
     string RefreshToken
 ) : IRequest<ResultT<LoginResponse>>;
}
