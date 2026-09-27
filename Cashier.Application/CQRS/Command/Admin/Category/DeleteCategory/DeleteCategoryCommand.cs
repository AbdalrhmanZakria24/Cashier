using MediatR;

namespace Cashier.Application.CQRS.Command.Category.DeleteCategory
{
    public record DeleteCategoryCommand(
        long Id
    ) : IRequest<ResultT<long>>;
}