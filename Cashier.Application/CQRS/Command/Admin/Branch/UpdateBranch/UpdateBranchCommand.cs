using MediatR;

namespace Cashier.Application.CQRS.Command.Branch.UpdateBranch
{
    public record UpdateBranchCommand(
        long Id,
        string? Name,
        string? Address,
        bool? IsActive
    ) : IRequest<ResultT<long>>;
}