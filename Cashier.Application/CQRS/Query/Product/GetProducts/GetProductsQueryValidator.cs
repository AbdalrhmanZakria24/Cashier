using FluentValidation;

namespace Cashier.Application.CQRS.Query.Product.GetProducts
{
    public class GetProductsQueryValidator
        : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(x => x.Pagination.PageNumber)
                .GreaterThanOrEqualTo(1)
                .When(x => x.Pagination is not null);

            RuleFor(x => x.Pagination.PageSize)
                .GreaterThanOrEqualTo(1)
                .When(x => x.Pagination is not null);

            RuleFor(x => x.Product)
                .Must(x => x == null || x.MinPrice <= x.MaxPrice)
                .WithMessage("Min price cannot be greater than max price");
        }
    }
}