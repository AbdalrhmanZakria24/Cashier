using FluentValidation;

namespace Cashier.Application.CQRS.Command.Product.DeleteProduct
{
    public class DeleteProductCommandValidator
        : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.BranchId)
                .GreaterThan(0);
        }
    }
}