using MediatR;

namespace Cashier.Application.CQRS.Command.Product.CreateProduct
{
    public record CreateProductCommand(long CategoryId,
        string Name,
        decimal Price,
        decimal Cost,
        List<AddProductBranch> AddProductBranches
    ) : IRequest<ResultT<long>>;
}