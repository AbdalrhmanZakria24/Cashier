using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.IncreaseCartItem
{
    public record IncreaseCartItemCommand(
        long ProductId,
        string UserId
    ) : IRequest<ResultT<bool>>;
}
