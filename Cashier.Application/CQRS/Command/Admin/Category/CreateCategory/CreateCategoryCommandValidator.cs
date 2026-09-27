using FluentValidation;

namespace Cashier.Application.CQRS.Command.Category.CreateCategory
{
    public class CreateCategoryCommandValidator
        : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}