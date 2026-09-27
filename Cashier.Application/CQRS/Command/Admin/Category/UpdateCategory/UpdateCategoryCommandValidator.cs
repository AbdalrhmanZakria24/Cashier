using FluentValidation;

namespace Cashier.Application.CQRS.Command.Category.UpdateCategory
{
    public class UpdateCategoryCommandValidator
        : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Name));
        }
    }
}