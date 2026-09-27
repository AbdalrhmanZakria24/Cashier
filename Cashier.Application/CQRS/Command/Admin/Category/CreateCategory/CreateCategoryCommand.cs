using MediatR;

namespace Cashier.Application.CQRS.Command.Category.CreateCategory
{
    public record CreateCategoryCommand(
        string Name
    ) : IRequest<ResultT<long>>;
}