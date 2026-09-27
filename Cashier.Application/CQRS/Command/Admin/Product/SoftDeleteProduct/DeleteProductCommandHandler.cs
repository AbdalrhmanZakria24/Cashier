using MediatR;

namespace Cashier.Application.CQRS.Command.Product.DeleteProduct
{
    public class DeleteProductCommandHandler
        : IRequestHandler<DeleteProductCommand, ResultT<long>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<long>> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var branchProduct =
                await _unitOfWork.BranchProductReposatory
                    .GetOneAsync(x =>
                        x.ProductId == request.ProductId &&
                        x.BranchId == request.BranchId);

            if (branchProduct is null)
            {
                return ResultT<long>.Failure(
                    new Error(
                        "Product.NotFound",
                        "Product not found in this branch",
                        ErrorType.NotFound));
            }

            if (branchProduct.IsDeleted)
            {
                return ResultT<long>.Success(branchProduct.Id);
            }

            branchProduct.IsDeleted = true;

            await _unitOfWork.CommitAsync(cancellationToken);

            return ResultT<long>.Success(branchProduct.Id);
        }
    }
}