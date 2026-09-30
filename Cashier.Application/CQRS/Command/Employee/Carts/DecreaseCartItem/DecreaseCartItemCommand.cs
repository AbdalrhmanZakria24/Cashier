using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.DecreaseCartItem
{
    public record DecreaseCartItemCommand(
       long ProductId,
       string UserId
   ) : IRequest<ResultT<bool>>;
}
