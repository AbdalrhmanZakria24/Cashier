using FluentValidation;

namespace Cashier.Application.CQRS.Command.Branch.UpdateBranch
{
    public class UpdateBranchCommandValidator
        : AbstractValidator<UpdateBranchCommand>
    {
        public UpdateBranchCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Address)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Address));
        }
    }
}