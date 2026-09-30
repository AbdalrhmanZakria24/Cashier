using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Query.Carts.GetCart
{
    public record GetCartQuery(
       string UserId,
       string? Code
   ) : IRequest<ResultT<CartResponseDto>>;
}
