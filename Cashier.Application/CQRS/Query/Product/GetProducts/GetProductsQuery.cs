using MediatR;

namespace Cashier.Application.CQRS.Query.Product.GetProducts
{
    public record GetProductsQuery(
        ProductSearch? Product,
        Pagination Pagination
    ) : IRequest<ResultT<ProductResponse>>;
}