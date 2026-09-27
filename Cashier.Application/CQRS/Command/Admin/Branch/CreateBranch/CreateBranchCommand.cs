using MediatR;

namespace Cashier.Application.CQRS.Command.Admin.Branch.CreateBranch
{
    public record CreateBranchCommand(
        string Name,
        string Address,
        bool IsActive
    ) : IRequest<ResultT<long>>;
}