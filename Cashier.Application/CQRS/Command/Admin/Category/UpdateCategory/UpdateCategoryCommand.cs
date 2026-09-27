using MediatR;

namespace Cashier.Application.CQRS.Command.Category.UpdateCategory
{
    public record UpdateCategoryCommand(
        long Id,
        string? Name
    ) : IRequest<ResultT<long>>;
}