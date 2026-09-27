using MediatR;

namespace Cashier.Application.CQRS.Command.Branch.UpdateBranch
{
    public class UpdateBranchCommandHandler
        : IRequestHandler<UpdateBranchCommand, ResultT<long>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<long>> Handle(
            UpdateBranchCommand request,
            CancellationToken cancellationToken)
        {

            var branch =
                await _unitOfWork.BranchReposatory
                    .GetOneAsync(x =>x.Id == request.Id);

            if (branch is null)
            {
                return ResultT<long>.Failure(
                    new Error(
                        "Branch.NotFound",
                        "Branch not found for this tenant",
                        ErrorType.NotFound));
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                branch.Name = request.Name;
            }

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                branch.Address = request.Address;
            }

            if (request.IsActive.HasValue)
            {
                branch.IsActive = request.IsActive.Value;
            }

            await _unitOfWork.CommitAsync(
                cancellationToken);

            return ResultT<long>.Success(branch.Id);
        }
    }
}