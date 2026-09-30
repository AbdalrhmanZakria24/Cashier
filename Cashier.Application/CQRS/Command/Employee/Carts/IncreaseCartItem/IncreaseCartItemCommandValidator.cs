using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.IncreaseCartItem
{
    public class IncreaseCartItemCommandValidator
        : AbstractValidator<IncreaseCartItemCommand>
    {
        public IncreaseCartItemCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
