using MediatR;

namespace Cashier.Application.CQRS.Command.Product.UpdateProduct
{
    public class UpdateProductCommandHandler
        : IRequestHandler<UpdateProductCommand, ResultT<long>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<long>> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductReposatory
                .GetOneAsync(x => x.Id == request.Id);

            if (product is null)
            {
                return ResultT<long>.Failure(
                    new Error(
                        "Product.NotFound",
                        "Product not found",
                        ErrorType.NotFound));
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                product.Name = request.Name;
            }

            if (request.Price.HasValue)
            {
                product.Price = request.Price.Value;
            }

            if (request.Cost.HasValue)
            {
                product.Cost = request.Cost.Value;
            }

            if (request.IsActive.HasValue)
            {
                product.IsActive = request.IsActive.Value;
            }

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (request.AddProductBranches is not null)
                {
                    var branchIds = request.AddProductBranches
                        .Select(x => x.BranchId)
                        .ToList();

                    var existingProducts =
                        await _unitOfWork.BranchProductReposatory
                            .GetAsync(x =>
                                x.ProductId == request.Id &&
                                branchIds.Contains(x.BranchId));

                    var lookup = existingProducts
                        .ToDictionary(x => x.BranchId);

                    foreach (var branchDto in request.AddProductBranches)
                    {
                        if (lookup.TryGetValue(
                            branchDto.BranchId,
                            out var existing))
                        {
                            existing.Quantity = branchDto.Quantity;
                        }
                        else
                        {
                            await _unitOfWork.BranchProductReposatory
                                .CreateAsync(
                                    new BranchProduct
                                    {
                                        ProductId = request.Id,
                                        BranchId = branchDto.BranchId,
                                        Quantity = branchDto.Quantity
                                    },
                                    cancellationToken);
                        }
                    }
                }

                await _unitOfWork.CommitAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return ResultT<long>.Success(product.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                return ResultT<long>.Failure(
                    new Error(
                        "Product.UpdateFailed",
                        ex.Message,
                        ErrorType.Failure));
            }
        }
    }
}