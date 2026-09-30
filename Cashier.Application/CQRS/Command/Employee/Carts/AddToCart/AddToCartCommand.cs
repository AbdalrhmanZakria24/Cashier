using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.AddToCart
{
    public record AddToCartCommand(
       long ProductId,
       string UserId,
       decimal Quantity
   ) : IRequest<ResultT<bool>>;
}
