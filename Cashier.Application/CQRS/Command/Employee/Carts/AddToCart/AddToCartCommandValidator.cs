using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.CQRS.Command.Employee.Carts.AddToCart
{
    public class AddToCartCommandValidator
        : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
