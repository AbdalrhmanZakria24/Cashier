using FluentValidation;

namespace Cashier.Application.CQRS.Command.Product.UpdateProduct
{
    public class UpdateProductCommandValidator
        : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Price.HasValue);

            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Cost.HasValue);

            RuleForEach(x => x.AddProductBranches)
                .ChildRules(branch =>
                {
                    branch.RuleFor(x => x.BranchId)
                        .GreaterThan(0);

                    branch.RuleFor(x => x.Quantity)
                        .GreaterThanOrEqualTo(0);
                });
        }
    }
}