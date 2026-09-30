using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.RemoveFromCart
{
    public record RemoveFromCartCommand(
         long ProductId,
         string UserId
     ) : IRequest<ResultT<bool>>;
}
