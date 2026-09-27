using FluentValidation;

namespace Cashier.Application.CQRS.Command.CreateProduct
{
    public class CreateProductCommandValidator
        : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.AddProductBranches)
                .NotNull();
        }
    }
}