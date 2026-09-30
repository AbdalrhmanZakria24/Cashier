using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.DecreaseCartItem
{
    public class DecreaseCartItemCommandValidator
         : AbstractValidator<DecreaseCartItemCommand>
    {
        public DecreaseCartItemCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
