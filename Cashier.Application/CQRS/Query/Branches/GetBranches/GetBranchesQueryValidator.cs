using FluentValidation;

namespace Cashier.Application.CQRS.Query.Branch.GetBranches
{
    public class GetBranchesQueryValidator
        : AbstractValidator<GetBranchesQuery>
    {
        public GetBranchesQueryValidator()
        {
            RuleFor(x => x.Pagination.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.Pagination.PageSize)
                .GreaterThanOrEqualTo(1);
        }
    }
}