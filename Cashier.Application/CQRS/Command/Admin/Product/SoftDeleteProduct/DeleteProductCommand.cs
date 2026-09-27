using MediatR;

namespace Cashier.Application.CQRS.Command.Product.DeleteProduct
{
    public record DeleteProductCommand(
        long ProductId,
        long BranchId
    ) : IRequest<ResultT<long>>;
}