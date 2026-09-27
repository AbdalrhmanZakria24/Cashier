using MediatR;

namespace Cashier.Application.CQRS.Query.Category.GetCategories
{
    public record GetCategoriesQuery(
        string? Name,
        Pagination Pagination
    ) : IRequest<ResultT<CategoryResponse>>;
}