using MediatR;

namespace Cashier.Application.CQRS.Command.Product.CreateProduct
{
    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, ResultT<long>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<long>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {

            var category = await _unitOfWork.Categoryreposatory
                .GetOneAsync(c => c.Id == request.CategoryId);

            if (category == null)
            {
                return ResultT<long>.Failure(
                    new Error(
                        "Product.CategoryNotFound",
                        "No category found",
                        ErrorType.NotFound));
            }

            var product = await _unitOfWork.ProductReposatory
                .GetOneAsync(x =>
                    x.Name == request.Name &&
                    x.CategoryId == request.CategoryId);

            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (product == null)
                {
                    product = new Cashier.Domain.Entities.Product
                    {
                        Name = request.Name,
                        Price = request.Price,
                        Cost = request.Cost,
                        CategoryId = request.CategoryId,
                        IsActive = true,
                        Version =1
                    };

                    await _unitOfWork.ProductReposatory
                        .CreateAsync(product, cancellationToken);

                    await _unitOfWork.CommitAsync(cancellationToken);
                }

                var branchIds = request.AddProductBranches
                    .Select(x => x.BranchId)
                    .ToList();

                var existingProducts =
                    await _unitOfWork.BranchProductReposatory
                        .GetAsync(x =>
                            x.ProductId == product.Id &&
                            branchIds.Contains(x.BranchId));

                var lookup = existingProducts
                    .ToDictionary(x => x.BranchId);

                foreach (var branchDto in request.AddProductBranches)
                {
                    if (lookup.TryGetValue(
                        branchDto.BranchId,
                        out var existing))
                    {
                        existing.Quantity += branchDto.Quantity;
                    }
                    else
                    {
                        await _unitOfWork.BranchProductReposatory
                            .CreateAsync(
                                new BranchProduct
                                {
                                    ProductId = product.Id,
                                    BranchId = branchDto.BranchId,
                                    Quantity = branchDto.Quantity
                                },
                                cancellationToken);
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
                        "Product.CreateFailed",
                        ex.Message,
                        ErrorType.Failure));
            }
        }
    }
}