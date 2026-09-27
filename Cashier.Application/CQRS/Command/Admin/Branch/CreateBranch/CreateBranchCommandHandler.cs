using MediatR;

namespace Cashier.Application.CQRS.Command.Admin.Branch.CreateBranch
{
    public class CreateBranchCommandHandler
        : IRequestHandler<CreateBranchCommand, ResultT<long>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBranchCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<long>> Handle(
            CreateBranchCommand request,
            CancellationToken cancellationToken)
        {

            var branch =
                await _unitOfWork.BranchReposatory
                    .GetOneAsync(x =>
                        x.Address.ToLower() ==
                        request.Address.ToLower());

            if (branch is not null)
            {
                return ResultT<long>.Failure(
                    new Error(
                        "Branch.AlreadyExists",
                        "Branch already exists",
                        ErrorType.BadRequest));
            }

            var newBranch = new Domain.Entities.Branch
            {
                Name = request.Name,
                Address = request.Address,
                IsActive = request.IsActive
            };

            try
            {
                await _unitOfWork.BranchReposatory
                    .CreateAsync(
                        newBranch,
                        cancellationToken);

                await _unitOfWork.CommitAsync(
                    cancellationToken);

                return ResultT<long>.Success(newBranch.Id);
            }
            catch (Exception ex)
            {
                return ResultT<long>.Failure(
                    new Error(
                        "Branch.CreateFailed",
                        ex.Message,
                        ErrorType.Failure));
            }
        }
    }
}