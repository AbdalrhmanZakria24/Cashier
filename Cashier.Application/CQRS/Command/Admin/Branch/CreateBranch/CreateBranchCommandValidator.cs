using FluentValidation;

namespace Cashier.Application.CQRS.Command.Admin.Branch.CreateBranch
{
    public class CreateBranchCommandValidator
        : AbstractValidator<CreateBranchCommand>
    {
        public CreateBranchCommandValidator()
        {
    
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}