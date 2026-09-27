using MediatR;

namespace Cashier.Application.CQRS.Command.Product.UpdateProduct
{
    public record UpdateProductCommand(
        long Id,
        string? Name,
        decimal? Price,
        decimal? Cost,
        bool? IsActive,
        List<AddProductBranch>? AddProductBranches
    ) : IRequest<ResultT<long>>;
}