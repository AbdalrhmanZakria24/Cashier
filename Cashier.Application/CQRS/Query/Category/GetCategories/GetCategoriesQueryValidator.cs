using FluentValidation;

namespace Cashier.Application.CQRS.Query.Category.GetCategories
{
    public class GetCategoriesQueryValidator
        : AbstractValidator<GetCategoriesQuery>
    {
        public GetCategoriesQueryValidator()
        {
            RuleFor(x => x.Pagination.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.Pagination.PageSize)
                .GreaterThanOrEqualTo(1);
        }
    }
}