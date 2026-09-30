using MediatR;

namespace Cashier.Application.CQRS.Query.Branch.GetBranches
{
    public record GetBranchesQuery(
        BranchSerch BranchSerc,
        Pagination Pagination
    ) : IRequest<ResultT<BranshResponse>>;
}