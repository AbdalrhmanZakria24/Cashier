using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.RemoveFromCart
{
    public class RemoveFromCartCommandValidator
         : AbstractValidator<RemoveFromCartCommand>
    {
        public RemoveFromCartCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
