using FluentValidation;

namespace Cashier.Application.CQRS.Command.Category.DeleteCategory
{
    public class DeleteCategoryCommandValidator
        : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}